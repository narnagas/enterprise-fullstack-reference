import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, GridApi, GridReadyEvent, RowClickedEvent, SortChangedEvent } from 'ag-grid-community';
import { finalize } from 'rxjs';
import { CustomerEditorComponent } from './customer-editor.component';
import { CustomerSearchRequest, CustomerSummary } from './customer.models';
import { CustomerService } from './customer.service';

@Component({
  selector: 'app-customer-search',
  standalone: true,
  imports: [FormsModule, AgGridAngular, CustomerEditorComponent],
  templateUrl: './customer-search.component.html',
  styleUrl: './customer-search.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerSearchComponent implements OnInit {
  searchText = '';
  state = '';
  activeFilter: 'active' | 'inactive' | 'all' = 'active';
  pageNumber = 1;
  pageSize = 25;
  totalCount = 0;
  totalPages = 0;
  loading = false;
  error = '';
  rowData: CustomerSummary[] = [];
  selectedCustomerId: number | null = null;
  private gridApi?: GridApi<CustomerSummary>;
  private sortField = 'companyName';
  private sortDirection: 'asc' | 'desc' = 'asc';

  readonly columnDefs: ColDef<CustomerSummary>[] = [
    { field: 'customerNumber', headerName: 'Customer #', sortable: true, width: 140 },
    { field: 'companyName', headerName: 'Company', sortable: true, minWidth: 220, flex: 1 },
    { field: 'contactName', headerName: 'Contact', sortable: true, minWidth: 180 },
    { field: 'email', headerName: 'Email', minWidth: 220, flex: 1 },
    { field: 'phone', headerName: 'Phone', width: 150 },
    { field: 'city', headerName: 'City', sortable: true, minWidth: 150 },
    { field: 'state', headerName: 'State', sortable: true, width: 100 },
    { field: 'isActive', headerName: 'Active', width: 100, valueFormatter: p => p.value ? 'Yes' : 'No' }
  ];

  readonly defaultColDef: ColDef<CustomerSummary> = { resizable: true, suppressHeaderMenuButton: true };

  constructor(private readonly customerService: CustomerService, private readonly cdr: ChangeDetectorRef) {}

  ngOnInit(): void { this.load(); }
  onGridReady(event: GridReadyEvent<CustomerSummary>): void { this.gridApi = event.api; }
  onRowClicked(event: RowClickedEvent<CustomerSummary>): void { this.selectedCustomerId = event.data?.id ?? null; }
  onCustomerSaved(): void { this.load(); }

  onSearch(): void { this.pageNumber = 1; this.load(); }

  onReset(): void {
    this.searchText = '';
    this.state = '';
    this.activeFilter = 'active';
    this.pageNumber = 1;
    this.selectedCustomerId = null;
    this.sortField = 'companyName';
    this.sortDirection = 'asc';
    this.gridApi?.applyColumnState({ defaultState: { sort: null }, state: [{ colId: 'companyName', sort: 'asc' }] });
    this.load();
  }

  onSortChanged(event: SortChangedEvent<CustomerSummary>): void {
    const sort = event.api.getColumnState().find(column => column.sort);
    if (!sort?.colId || !sort.sort) return;
    this.sortField = sort.colId;
    this.sortDirection = sort.sort;
    this.pageNumber = 1;
    this.load();
  }

  previousPage(): void { if (this.pageNumber > 1 && !this.loading) { this.pageNumber--; this.load(); } }
  nextPage(): void { if (this.pageNumber < this.totalPages && !this.loading) { this.pageNumber++; this.load(); } }
  onPageSizeChange(): void { this.pageNumber = 1; this.load(); }

  private load(): void {
    this.loading = true;
    this.error = '';
    const request: CustomerSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      search: this.searchText.trim() || null,
      state: this.state.trim() || null,
      isActive: this.activeFilter === 'all' ? null : this.activeFilter === 'active',
      sortField: this.sortField,
      sortDirection: this.sortDirection
    };

    this.customerService.search(request)
      .pipe(finalize(() => { this.loading = false; this.cdr.markForCheck(); }))
      .subscribe({
        next: response => {
          this.rowData = response.items;
          this.totalCount = response.totalCount;
          this.totalPages = response.totalPages;
          this.pageNumber = response.pageNumber;
        },
        error: () => {
          this.rowData = [];
          this.totalCount = 0;
          this.totalPages = 0;
          this.error = 'Unable to load customers. Please try again.';
        }
      });
  }
}
