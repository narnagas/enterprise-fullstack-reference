import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { ProjectEditorComponent } from './project-editor.component';
import { ProjectDetail } from './project.models';
import { environment } from '../../../environments/environment';

describe('ProjectEditorComponent', () => {
  let fixture: ComponentFixture<ProjectEditorComponent>;
  let component: ProjectEditorComponent;
  let http: HttpTestingController;

  const project: ProjectDetail = {
    id: 1,
    projectNumber: 'PRJ-1001',
    name: 'North Modernization',
    customerName: 'Northwind Industries',
    status: 'Active',
    createdDate: '2026-09-01T00:00:00Z',
    dueDate: '2026-09-30T00:00:00Z',
    isActive: true
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectEditorComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()]
    }).compileComponents();

    fixture = TestBed.createComponent(ProjectEditorComponent);
    component = fixture.componentInstance;
    http = TestBed.inject(HttpTestingController);
    fixture.detectChanges();
  });

  afterEach(() => http.verify());

  function loadProject(): void {
    component.projectId = 1;
    const request = http.expectOne(`${environment.apiUrl}/projects/1`);
    expect(request.request.method).toBe('GET');
    request.flush(project);
    fixture.detectChanges();
  }

  it('loads the selected project and populates the form', () => {
    loadProject();

    expect(component.project).toEqual(project);
    expect(component.form.getRawValue()).toEqual({
      name: 'North Modernization',
      customerName: 'Northwind Industries',
      status: 'Active',
      dueDate: '2026-09-30',
      isActive: true
    });
    expect(component.form.pristine).toBeTrue();
  });

  it('prevents save when required fields are invalid', () => {
    loadProject();
    component.form.controls.name.setValue('');

    component.save();

    expect(component.form.invalid).toBeTrue();
    expect(component.form.controls.name.touched).toBeTrue();
    http.expectNone(`${environment.apiUrl}/projects/1`);
  });

  it('saves edited values and emits the saved project', () => {
    loadProject();
    const emitted: ProjectDetail[] = [];
    component.saved.subscribe(value => emitted.push(value));
    component.form.patchValue({
      name: '  North Platform Modernization  ',
      customerName: '  Northwind Enterprise  ',
      status: 'Planning',
      dueDate: '',
      isActive: false
    });

    component.save();

    const request = http.expectOne(`${environment.apiUrl}/projects/1`);
    expect(request.request.method).toBe('PUT');
    expect(request.request.body).toEqual({
      name: 'North Platform Modernization',
      customerName: 'Northwind Enterprise',
      status: 'Planning',
      dueDate: null,
      isActive: false
    });

    const saved: ProjectDetail = {
      ...project,
      name: 'North Platform Modernization',
      customerName: 'Northwind Enterprise',
      status: 'Planning',
      dueDate: null,
      isActive: false
    };
    request.flush(saved);
    fixture.detectChanges();

    expect(component.project).toEqual(saved);
    expect(component.success).toBe('Project saved.');
    expect(component.form.pristine).toBeTrue();
    expect(emitted).toEqual([saved]);
  });

  it('cancel restores the last loaded project values', () => {
    loadProject();
    component.form.patchValue({ name: 'Unsaved Name', status: 'On Hold' });
    expect(component.form.dirty).toBeTrue();

    component.cancel();

    expect(component.form.controls.name.value).toBe('North Modernization');
    expect(component.form.controls.status.value).toBe('Active');
    expect(component.form.pristine).toBeTrue();
  });

  it('shows a load error when the project request fails', () => {
    component.projectId = 404;
    const request = http.expectOne(`${environment.apiUrl}/projects/404`);
    request.flush({}, { status: 404, statusText: 'Not Found' });
    fixture.detectChanges();

    expect(component.project).toBeNull();
    expect(component.error).toBe('Unable to load the selected project.');
  });

  it('keeps edits and shows an error when save fails', () => {
    loadProject();
    component.form.controls.name.setValue('Updated Name');

    component.save();
    const request = http.expectOne(`${environment.apiUrl}/projects/1`);
    request.flush({}, { status: 500, statusText: 'Server Error' });
    fixture.detectChanges();

    expect(component.form.controls.name.value).toBe('Updated Name');
    expect(component.error).toBe('Unable to save the project. Please try again.');
    expect(component.success).toBe('');
  });
});
