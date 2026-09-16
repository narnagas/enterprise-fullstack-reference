import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { ProjectDetail, UpdateProjectRequest } from './project.models';
import { ProjectService } from './project.service';

@Component({
  selector: 'app-project-editor',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './project-editor.component.html',
  styleUrl: './project-editor.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProjectEditorComponent {
  private selectedProjectId: number | null = null;
  project: ProjectDetail | null = null;
  loading = false;
  saving = false;
  error = '';
  success = '';

  @Output() readonly saved = new EventEmitter<ProjectDetail>();

  readonly form;

  @Input()
  set projectId(value: number | null) {
    if (value === this.selectedProjectId) return;
    this.selectedProjectId = value;
    value ? this.load(value) : this.clear();
  }

  constructor(
    private readonly fb: FormBuilder,
    private readonly projectService: ProjectService,
    private readonly cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.nonNullable.group({
      name: ['', [Validators.required, Validators.maxLength(150)]],
      customerName: ['', [Validators.required, Validators.maxLength(150)]],
      status: ['', [Validators.required, Validators.maxLength(50)]],
      dueDate: [''],
      isActive: [true]
    });
  }

  save(): void {
    if (!this.project || this.form.invalid || this.saving) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.error = '';
    this.success = '';
    const value = this.form.getRawValue();
    const request: UpdateProjectRequest = {
      name: value.name.trim(),
      customerName: value.customerName.trim(),
      status: value.status.trim(),
      dueDate: value.dueDate || null,
      isActive: value.isActive
    };

    this.projectService.update(this.project.id, request)
      .pipe(finalize(() => {
        this.saving = false;
        this.cdr.markForCheck();
      }))
      .subscribe({
        next: project => {
          this.project = project;
          this.patchForm(project);
          this.success = 'Project saved.';
          this.saved.emit(project);
        },
        error: () => this.error = 'Unable to save the project. Please try again.'
      });
  }

  cancel(): void {
    if (!this.project) return;
    this.patchForm(this.project);
    this.error = '';
    this.success = '';
  }

  private load(id: number): void {
    this.loading = true;
    this.error = '';
    this.success = '';
    this.project = null;

    this.projectService.getById(id)
      .pipe(finalize(() => {
        this.loading = false;
        this.cdr.markForCheck();
      }))
      .subscribe({
        next: project => {
          this.project = project;
          this.patchForm(project);
        },
        error: () => this.error = 'Unable to load the selected project.'
      });
  }

  private patchForm(project: ProjectDetail): void {
    this.form.reset({
      name: project.name,
      customerName: project.customerName,
      status: project.status,
      dueDate: this.toDateInput(project.dueDate),
      isActive: project.isActive
    });
  }

  private clear(): void {
    this.project = null;
    this.error = '';
    this.success = '';
    this.form.reset({ name: '', customerName: '', status: '', dueDate: '', isActive: true });
    this.cdr.markForCheck();
  }

  private toDateInput(value: string | null): string {
    return value ? value.substring(0, 10) : '';
  }
}
