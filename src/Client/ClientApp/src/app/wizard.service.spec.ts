import { TestBed, getTestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { WizardService } from './wizard.service';
import { Node } from './models';

describe('WizardService', () => {
  let injector: TestBed;
  let service: WizardService;
  let httpMock: HttpTestingController;
  let baseUrl: 'http://localhost:5000';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        WizardService,
        { provide: 'BASE_API_URL', useValue: baseUrl, deps: [] }
      ]
    });

    injector = getTestBed();
    service = injector.get(WizardService);
    httpMock = injector.get(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('loads steps from web service', () => {
    let loadedSteps;
    service.loadSteps((steps) => { loadedSteps = steps; }, () => { });

    const request = httpMock.expectOne({ method: 'GET', url: baseUrl+'/wizard/steps' });
    request.flush([
      { Id: 'Step1', ControlName: 'Step1Control', Title: 'Step 1' },
      { Id: 'Step2', ControlName: 'Step2Control', Title: 'Step 2' }
    ]);

    expect(loadedSteps.length).toBe(2, 'Wrong number of steps returned.');
    expect(loadedSteps[0].Id).toBe('Step1');
    expect(loadedSteps[1].Id).toBe('Step2');
  });

  it('calls to web service on next()', () => {
    service.next(null, () => { }, () => { });

    const request = httpMock.expectOne({ method: 'POST', url: baseUrl +'/wizard/next' });
    request.flush({ CanTransition: true });
  });

  it('sends node to web service on next()', () => {
    const node = new Node();
    service.next(node, () => { }, () => { });

    const request = httpMock.expectOne({ method: 'POST', url: baseUrl + '/wizard/next' });
    request.flush({ CanTransition: true });

    expect(request.request.body).toBe(node);
  });

  it('returns server response on next()', () => {
    let returnedResult;
    service.next(null, (response) => {
      returnedResult = response;
    }, () => { });

    const request = httpMock.expectOne({ method: 'POST', url: baseUrl + '/wizard/next' });
    request.flush({ CanTransition: true });

    expect(returnedResult.CanTransition).toBeTruthy('expected response should allow transition');
  });

  it('calls error handler on error in next()', () => {
    let returnedError;
    service.next(null, () => { }, (error) => {
      returnedError = error;
    });

    const request = httpMock.expectOne({ method: 'POST', url: baseUrl + '/wizard/next' });
    request.flush('', { status: 500, statusText: 'test error' });

    expect(returnedError).toBe('test error');
  });

  it('calls to web service on back()', () => {
    service.back(() => { }, () => { });

    const request = httpMock.expectOne({ method: 'POST', url: baseUrl + '/wizard/back' });
    request.flush({ CanTransition: true });
  });

  it('returns server response on back()', () => {
    let returnedResult;
    service.back((response) => {
      returnedResult = response;
    }, () => { });


    const request = httpMock.expectOne({ method: 'POST', url: baseUrl + '/wizard/back' });
    request.flush({ CanTransition: true });

    expect(returnedResult.CanTransition).toBeTruthy('expected response should allow transition');
  });

  it('calls error handler on error in back()', () => {
    let returnedError;
    service.back(() => { }, (error) => {
      returnedError = error;
    });

    const request = httpMock.expectOne({ method: 'POST', url: baseUrl + '/wizard/back' });
    request.flush('', { status: 500, statusText: 'test error' });

    expect(returnedError).toBe('test error');
  });

  it('calls web service on addNode()', () => {
    service.addNode(null, () => { }, () => { });

    const request = httpMock.expectOne({ method: 'POST', url: baseUrl + '/wizard/add' });
    request.flush('', { status: 200, statusText: '' });
  });

  it('sends node to web service on addNode()', () => {
    const node = new Node();
    service.addNode(node, () => { }, () => { });

    const request = httpMock.expectOne({ method: 'POST', url: baseUrl + '/wizard/add' });
    request.flush('', { status: 200, statusText: '' });

    expect(request.request.body).toBe(node);
  });

  // ... more tests for rest of the methods ...
});
