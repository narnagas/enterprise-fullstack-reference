import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../core/auth/auth.service';
import { CustomerDetail } from './customer.models';
import { CustomerEditorComponent } from './customer-editor.component';

describe('CustomerEditorComponent', () => {
  let fixture: ComponentFixture<CustomerEditorComponent>;
  let component: CustomerEditorComponent;
  let http: HttpTestingController;
  let auth: AuthService;

  const customer: CustomerDetail = { id: 1, customerNumber: 'CUST-1001', companyName: 'Northwind Industries', contactName: 'Avery Stone', email: 'avery@example.test', phone: '713-555-0101', city: 'Houston', state: 'TX', createdDate: '2026-08-01T00:00:00Z', isActive: true };

  beforeEach(async () => {
    await TestBed.configureTestingModule({ imports: [CustomerEditorComponent], providers: [provideHttpClient(), provideHttpClientTesting()] }).compileComponents();
    fixture = TestBed.createComponent(CustomerEditorComponent);
    component = fixture.componentInstance;
    http = TestBed.inject(HttpTestingController);
    auth = TestBed.inject(AuthService);
    auth.setRole('Editor');
    fixture.detectChanges();
  });

  afterEach(() => http.verify());

  function loadCustomer(): void {
    component.customerId = 1;
    const request = http.expectOne(`${environment.apiUrl}/customers/1`);
    expect(request.request.method).toBe('GET');
    request.flush(customer);
    fixture.detectChanges();
  }

  it('loads the selected customer and populates the form', () => {
    loadCustomer();
    expect(component.customer).toEqual(customer);
    expect(component.form.controls.companyName.value).toBe('Northwind Industries');
    expect(component.form.pristine).toBeTrue();
  });

  it('prevents save when the company name is missing', () => {
    loadCustomer();
    component.form.controls.companyName.setValue('');
    component.save();
    expect(component.form.invalid).toBeTrue();
    expect(component.form.controls.companyName.touched).toBeTrue();
    http.expectNone(`${environment.apiUrl}/customers/1`);
  });

  it('prevents a Viewer from issuing an update', () => {
    loadCustomer();
    auth.setRole('Viewer');
    component.form.controls.companyName.setValue('Blocked Update');
    component.save();
    http.expectNone(`${environment.apiUrl}/customers/1`);
    expect(component.saving).toBeFalse();
  });

  it('allows an Administrator to issue an update', () => {
    loadCustomer();
    auth.setRole('Administrator');
    component.form.controls.companyName.setValue('Authorized Update');
    component.save();
    const request = http.expectOne(`${environment.apiUrl}/customers/1`);
    expect(request.request.method).toBe('PUT');
    request.flush({ ...customer, companyName: 'Authorized Update' });
  });

  it('saves trimmed edits and emits the saved customer', () => {
    loadCustomer();
    const emitted: CustomerDetail[] = [];
    component.saved.subscribe(value => emitted.push(value));
    component.form.patchValue({ companyName: ' Northwind Enterprise ', contactName: ' Taylor Brooks ', email: 'taylor@example.test', isActive: false });
    component.save();
    const request = http.expectOne(`${environment.apiUrl}/customers/1`);
    expect(request.request.method).toBe('PUT');
    expect(request.request.body.companyName).toBe('Northwind Enterprise');
    expect(request.request.body.contactName).toBe('Taylor Brooks');
    expect(request.request.body.email).toBe('taylor@example.test');
    expect(request.request.body.isActive).toBeFalse();
    const saved = { ...customer, companyName: 'Northwind Enterprise', contactName: 'Taylor Brooks', email: 'taylor@example.test', isActive: false };
    request.flush(saved);
    fixture.detectChanges();
    expect(component.success).toBe('Customer saved.');
    expect(component.form.pristine).toBeTrue();
    expect(emitted).toEqual([saved]);
  });

  it('cancel restores the last loaded customer values', () => {
    loadCustomer();
    component.form.controls.companyName.setValue('Unsaved Company');
    component.form.markAsDirty();
    component.cancel();
    expect(component.form.controls.companyName.value).toBe('Northwind Industries');
    expect(component.form.pristine).toBeTrue();
  });

  it('shows a load error when the customer request fails', () => {
    component.customerId = 404;
    const request = http.expectOne(`${environment.apiUrl}/customers/404`);
    request.flush({}, { status: 404, statusText: 'Not Found' });
    fixture.detectChanges();
    expect(component.customer).toBeNull();
    expect(component.error).toBe('Unable to load the selected customer.');
  });

  it('preserves edits when save fails', () => {
    loadCustomer();
    component.form.controls.companyName.setValue('Updated Company');
    component.save();
    const request = http.expectOne(`${environment.apiUrl}/customers/1`);
    request.flush({}, { status: 500, statusText: 'Server Error' });
    fixture.detectChanges();
    expect(component.form.controls.companyName.value).toBe('Updated Company');
    expect(component.error).toBe('Unable to save the customer. Please try again.');
  });
});
