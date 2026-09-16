import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, GridReadyEvent, RowClickedEvent, SortChangedEvent } from 'ag-grid-community';
import { finalize } from 'rxjs';
import { ProjectEditorComponent } from './project-editor.component';
import { ProjectSearchRequest, ProjectSummary } from './project.models';
import { ProjectService } from './project.service';

@Component({
  selector: 'app-project-search',
  standalone: true,
  imports: [FormsModule, AgGridAngular, ProjectEditorComponent],
  templateUrl: './project-search.component.html',
  styleUrl: './project-search.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProjectSearchComponent implements OnInit {
  searchText = '';
  customer = '';
  status = '';
  activeFilter = 'active';
  pageNumber = 1;
  pageSize = 25;
  totalCount = 0;
  totalPages = 0;
  loading = false;
  error = '';
  selectedProjectId: number | null = null;
  rowData: ProjectSummary[] = [];
  sortField = 'createdDate';
  sortDirection: 'asc' | 'desc' = 'desc';

  readonly columnDefs: ColDef<ProjectSummary>[] = [
    { field: 'projectNumber', headerName: 'Project #', sortable: true },
    { field: 'name', flex: 1, minWidth: 180, sortable: true },
    { field: 'customerName', headerName: 'Customer', flex: 1, minWidth: 180, sortable: true },
    { field: 'status', sortable: true },
    { field: 'createdDate', headerName: 'Created', sortable: true, valueFormatter: p => this.formatDate(p.value) },
    { field: 'dueDate', headerName: 'Due', sortable: true, valueFormatter: p => this.formatDate(p.value) },
    { field: 'isActive', headerName: 'Active', sortable: false, valueFormatter: p => p.value ? 'Yes' : 'No' }
  ];

  readonly defaultColDef: ColDef = { resizable: true, minWidth: 110 };

  constructor(private readonly projectService: ProjectService, private readonly cdr: ChangeDetectorRef) {}

  ngOnInit(): void { this.loadProjects(); }
  onSearch(): void { this.pageNumber = 1; this.loadProjects(); }
  onPageSizeChange(): void { this.pageNumber = 1; this.loadProjects(); }

  onReset(): void {
    this.searchText = '';
    this.customer = '';
    this.status = '';
    this.activeFilter = 'active';
    this.pageNumber = 1;
    this.sortField = 'createdDate';
    this.sortDirection = 'desc';
    this.selectedProjectId = null;
    this.loadProjects();
  }

  previousPage(): void { if (this.pageNumber > 1) { this.pageNumber--; this.loadProjects(); } }
  nextPage(): void { if (this.pageNumber < this.totalPages) { this.pageNumber++; this.loadProjects(); } }
  onGridReady(_: GridReadyEvent<ProjectSummary>): void {}

  onRowClicked(event: RowClickedEvent<ProjectSummary>): void {
    this.selectedProjectId = event.data?.id ?? null;
  }

  onProjectSaved(): void { this.loadProjects(); }

  onSortChanged(event: SortChangedEvent<ProjectSummary>): void {
    const sorted = event.api.getColumnState().find(column => column.sort);
    if (!sorted?.colId || !sorted.sort) return;
    this.sortField = sorted.colId;
    this.sortDirection = sorted.sort;
    this.pageNumber = 1;
    this.loadProjects();
  }

  private loadProjects(): void {
    this.loading = true;
    this.error = '';
    const request: ProjectSearchRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      search: this.toNullable(this.searchText),
      customer: this.toNullable(this.customer),
      status: this.toNullable(this.status),
      isActive: this.activeFilter === 'all' ? null : this.activeFilter === 'active',
      sortField: this.sortField,
      sortDirection: this.sortDirection
    };

    this.projectService.search(request)
      .pipe(finalize(() => { this.loading = false; this.cdr.markForCheck(); }))
      .subscribe({
        next: response => {
          this.rowData = response.items;
          this.pageNumber = response.pageNumber;
          this.pageSize = response.pageSize;
          this.totalCount = response.totalCount;
          this.totalPages = response.totalPages;
        },
        error: () => {
          this.rowData = [];
          this.totalCount = 0;
          this.totalPages = 0;
          this.error = 'Unable to load projects. Verify that the API is running and accessible.';
        }
      });
  }

  private toNullable(value: string): string | null {
    const trimmed = value.trim();
    return trimmed.length ? trimmed : null;
  }

  private formatDate(value: string | null | undefined): string {
    if (!value) return '';
    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? '' : date.toLocaleDateString();
  }
}
