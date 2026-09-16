import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { environment } from '../../../environments/environment';
import { ProjectSearchRequest } from './project.models';
import { ProjectService } from './project.service';

class ProjectServiceTestSuite {
  static configure(): { service: ProjectService; http: HttpTestingController } {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), ProjectService]
    });

    return {
      service: TestBed.inject(ProjectService),
      http: TestBed.inject(HttpTestingController)
    };
  }
}

describe('ProjectService', () => {
  afterEach(() => TestBed.inject(HttpTestingController).verify());

  it('posts the typed search request to the projects search endpoint', () => {
    const { service, http } = ProjectServiceTestSuite.configure();
    const request: ProjectSearchRequest = {
      pageNumber: 1,
      pageSize: 25,
      search: 'north',
      customer: null,
      status: 'Active',
      isActive: true,
      sortField: 'createdDate',
      sortDirection: 'desc'
    };

    service.search(request).subscribe(response => {
      expect(response.totalCount).toBe(1);
      expect(response.items[0].projectNumber).toBe('PRJ-1001');
    });

    const pending = http.expectOne(`${environment.apiUrl}/projects/search`);
    expect(pending.request.method).toBe('POST');
    expect(pending.request.body).toEqual(request);

    pending.flush({
      items: [{
        id: 1,
        projectNumber: 'PRJ-1001',
        name: 'North Modernization',
        customerName: 'Northwind Industries',
        status: 'Active',
        createdDate: '2026-09-01T00:00:00Z',
        dueDate: null,
        isActive: true
      }],
      pageNumber: 1,
      pageSize: 25,
      totalCount: 1,
      totalPages: 1
    });
  });
});
