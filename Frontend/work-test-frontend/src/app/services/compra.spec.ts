import { TestBed } from '@angular/core/testing';

import { Compra } from './compra';

describe('Compra', () => {
  let service: Compra;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Compra);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
