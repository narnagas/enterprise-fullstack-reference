import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { environment } from '../../../environments/environment';
import { CustomerSearchRequest } from './customer.models';
import { CustomerService } from './customer.service';

describe('CustomerService', () => {
  let service: CustomerService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), CustomerService]
    });
    service = TestBed.inject(CustomerService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('posts a typed search request to the customers search endpoint', () => {
    const request: CustomerSearchRequest = {
      pageNumber: 1,
      pageSize: 25,
      search: 'north',
      state: 'TX',
      isActive: true,
      sortField: 'companyName',
      sortDirection: 'asc'
    };

    service.search(request).subscribe(response => {
      expect(response.totalCount).toBe(1);
      expect(response.items[0].customerNumber).toBe('CUST-1001');
    });

    const pending = http.expectOne(`${environment.apiUrl}/customers/search`);
    expect(pending.request.method).toBe('POST');
    expect(pending.request.body).toEqual(request);
    pending.flush({
      items: [{
        id: 1,
        customerNumber: 'CUST-1001',
        companyName: 'Northwind Industries',
        contactName: 'Avery Stone',
        email: 'avery@example.test',
        phone: '713-555-0101',
        city: 'Houston',
        state: 'TX',
        createdDate: '2026-08-01T00:00:00Z',
        isActive: true
      }],
      pageNumber: 1,
      pageSize: 25,
      totalCount: 1,
      totalPages: 1
    });
  });
});
