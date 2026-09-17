import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResponse } from '../../core/models/paged-response';
import { CustomerDetail, CustomerSearchRequest, CustomerSummary, UpdateCustomerRequest } from './customer.models';

@Injectable({ providedIn: 'root' })
export class CustomerService {
  private readonly apiUrl = `${environment.apiUrl}/customers`;

  constructor(private readonly http: HttpClient) {}

  search(request: CustomerSearchRequest): Observable<PagedResponse<CustomerSummary>> {
    return this.http.post<PagedResponse<CustomerSummary>>(`${this.apiUrl}/search`, request);
  }

  getById(id: number): Observable<CustomerDetail> {
    return this.http.get<CustomerDetail>(`${this.apiUrl}/${id}`);
  }

  update(id: number, request: UpdateCustomerRequest): Observable<CustomerDetail> {
    return this.http.put<CustomerDetail>(`${this.apiUrl}/${id}`, request);
  }
}
