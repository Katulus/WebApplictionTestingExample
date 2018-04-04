import { TestBed, getTestBed } from '@angular/core/testing';
import { NodesService } from './nodes.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('NodesService', () => {
  let injector: TestBed;
  let service: NodesService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        NodesService,
        { provide: 'BASE_API_URL', useValue: 'http://localhost:5000', deps: [] }]
    });

    injector = getTestBed();
    service = injector.get(NodesService);
    httpMock = injector.get(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
