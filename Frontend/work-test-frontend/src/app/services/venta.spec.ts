import { TestBed } from '@angular/core/testing';

import { Venta } from './venta';

describe('Venta', () => {
  let service: Venta;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Venta);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
