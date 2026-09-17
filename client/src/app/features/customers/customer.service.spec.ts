import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { environment } from '../../../environments/environment';
import { CustomerDetail, CustomerSearchRequest, UpdateCustomerRequest } from './customer.models';
import { CustomerService } from './customer.service';

describe('CustomerService', () => {
  let service: CustomerService;
  let http: HttpTestingController;

  const customer: CustomerDetail = {
    id: 1, customerNumber: 'CUST-1001', companyName: 'Northwind Industries', contactName: 'Avery Stone',
    email: 'avery@example.test', phone: '713-555-0101', city: 'Houston', state: 'TX',
    createdDate: '2026-08-01T00:00:00Z', isActive: true
  };

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting(), CustomerService] });
    service = TestBed.inject(CustomerService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('posts a typed search request to the customers search endpoint', () => {
    const request: CustomerSearchRequest = { pageNumber: 1, pageSize: 25, search: 'north', state: 'TX', isActive: true, sortField: 'companyName', sortDirection: 'asc' };
    service.search(request).subscribe(response => expect(response.items[0].customerNumber).toBe('CUST-1001'));
    const pending = http.expectOne(`${environment.apiUrl}/customers/search`);
    expect(pending.request.method).toBe('POST');
    expect(pending.request.body).toEqual(request);
    pending.flush({ items: [customer], pageNumber: 1, pageSize: 25, totalCount: 1, totalPages: 1 });
  });

  it('gets a customer by id', () => {
    service.getById(1).subscribe(response => expect(response).toEqual(customer));
    const pending = http.expectOne(`${environment.apiUrl}/customers/1`);
    expect(pending.request.method).toBe('GET');
    pending.flush(customer);
  });

  it('puts a typed customer update request', () => {
    const request: UpdateCustomerRequest = { companyName: 'Northwind Enterprise', contactName: 'Taylor Brooks', email: 'taylor@example.test', phone: '713-555-0199', city: 'Houston', state: 'TX', isActive: false };
    service.update(1, request).subscribe(response => expect(response.companyName).toBe('Northwind Enterprise'));
    const pending = http.expectOne(`${environment.apiUrl}/customers/1`);
    expect(pending.request.method).toBe('PUT');
    expect(pending.request.body).toEqual(request);
    pending.flush({ ...customer, ...request });
  });
});
