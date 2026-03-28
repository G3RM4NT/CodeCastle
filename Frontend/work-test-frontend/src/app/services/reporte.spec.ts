import { TestBed } from '@angular/core/testing';

import { Reporte } from './reporte';

describe('Reporte', () => {
  let service: Reporte;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Reporte);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
