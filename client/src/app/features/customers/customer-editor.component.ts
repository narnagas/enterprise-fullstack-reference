import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { CustomerDetail, UpdateCustomerRequest } from './customer.models';
import { CustomerService } from './customer.service';

@Component({
  selector: 'app-customer-editor',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './customer-editor.component.html',
  styleUrl: './customer-editor.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerEditorComponent {
  private selectedCustomerId: number | null = null;
  customer: CustomerDetail | null = null;
  loading = false;
  saving = false;
  error = '';
  success = '';

  @Output() readonly saved = new EventEmitter<CustomerDetail>();

  readonly form;

  @Input()
  set customerId(value: number | null) {
    if (value === this.selectedCustomerId) return;
    this.selectedCustomerId = value;
    value ? this.load(value) : this.clear();
  }

  constructor(
    private readonly fb: FormBuilder,
    private readonly customerService: CustomerService,
    private readonly cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.nonNullable.group({
      companyName: ['', [Validators.required, Validators.maxLength(150)]],
      contactName: ['', Validators.maxLength(150)],
      email: ['', [Validators.email, Validators.maxLength(200)]],
      phone: ['', Validators.maxLength(50)],
      city: ['', Validators.maxLength(100)],
      state: ['', Validators.maxLength(50)],
      isActive: [true]
    });
  }

  save(): void {
    if (!this.customer || this.form.invalid || this.saving) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.error = '';
    this.success = '';
    const value = this.form.getRawValue();
    const request: UpdateCustomerRequest = {
      companyName: value.companyName.trim(),
      contactName: value.contactName.trim(),
      email: value.email.trim(),
      phone: value.phone.trim(),
      city: value.city.trim(),
      state: value.state.trim(),
      isActive: value.isActive
    };

    this.customerService.update(this.customer.id, request)
      .pipe(finalize(() => {
        this.saving = false;
        this.cdr.markForCheck();
      }))
      .subscribe({
        next: customer => {
          this.customer = customer;
          this.patchForm(customer);
          this.success = 'Customer saved.';
          this.saved.emit(customer);
        },
        error: () => this.error = 'Unable to save the customer. Please try again.'
      });
  }

  cancel(): void {
    if (!this.customer) return;
    this.patchForm(this.customer);
    this.error = '';
    this.success = '';
  }

  private load(id: number): void {
    this.loading = true;
    this.error = '';
    this.success = '';
    this.customer = null;

    this.customerService.getById(id)
      .pipe(finalize(() => {
        this.loading = false;
        this.cdr.markForCheck();
      }))
      .subscribe({
        next: customer => {
          this.customer = customer;
          this.patchForm(customer);
        },
        error: () => this.error = 'Unable to load the selected customer.'
      });
  }

  private patchForm(customer: CustomerDetail): void {
    this.form.reset({
      companyName: customer.companyName,
      contactName: customer.contactName,
      email: customer.email,
      phone: customer.phone,
      city: customer.city,
      state: customer.state,
      isActive: customer.isActive
    });
  }

  private clear(): void {
    this.customer = null;
    this.error = '';
    this.success = '';
    this.form.reset({ companyName: '', contactName: '', email: '', phone: '', city: '', state: '', isActive: true });
    this.cdr.markForCheck();
  }
}
