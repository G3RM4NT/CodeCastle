import { TestBed } from '@angular/core/testing';

import { Cliente } from './cliente.service';

describe('Cliente', () => {
  let service: Cliente;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Cliente);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
