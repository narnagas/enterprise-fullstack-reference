import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResponse } from '../../core/models/paged-response';
import { ProjectSearchRequest, ProjectSummary } from './project.models';

@Injectable({ providedIn: 'root' })
export class ProjectService {
  private readonly apiUrl = 'https://localhost:7001/api/projects';

  constructor(private readonly http: HttpClient) {}

  search(request: ProjectSearchRequest): Observable<PagedResponse<ProjectSummary>> {
    return this.http.post<PagedResponse<ProjectSummary>>(
      `${this.apiUrl}/search`,
      request
    );
  }
}
