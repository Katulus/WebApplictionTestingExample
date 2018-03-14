import { TestBed, inject } from '@angular/core/testing';

import { HttpServiceBaseService } from './http-service-base.service';

describe('HttpServiceBaseService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HttpServiceBaseService]
    });
  });

  it('should be created', inject([HttpServiceBaseService], (service: HttpServiceBaseService) => {
    expect(service).toBeTruthy();
  }));
});
