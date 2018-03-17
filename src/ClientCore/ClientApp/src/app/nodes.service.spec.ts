import { TestBed, inject, getTestBed } from '@angular/core/testing';

import { NodesService } from './nodes.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('NodesService', () => {
  let injector: TestBed;
  let service: NodesService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [NodesService]
    });

    injector = getTestBed();
    service = injector.get(NodesService);
    httpMock = injector.get(HttpTestingController);
  });

  it('should be created', inject([NodesService], (service: NodesService) => {
    expect(service).toBeTruthy();
  }));
});
