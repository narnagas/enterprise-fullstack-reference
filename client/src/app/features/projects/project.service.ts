import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResponse } from '../../core/models/paged-response';
import { ProjectDetail, ProjectSearchRequest, ProjectSummary, UpdateProjectRequest } from './project.models';

@Injectable({ providedIn: 'root' })
export class ProjectService {
  private readonly apiUrl = `${environment.apiUrl}/projects`;

  constructor(private readonly http: HttpClient) {}

  search(request: ProjectSearchRequest): Observable<PagedResponse<ProjectSummary>> {
    return this.http.post<PagedResponse<ProjectSummary>>(`${this.apiUrl}/search`, request);
  }

  getById(id: number): Observable<ProjectDetail> {
    return this.http.get<ProjectDetail>(`${this.apiUrl}/${id}`);
  }

  update(id: number, request: UpdateProjectRequest): Observable<ProjectDetail> {
    return this.http.put<ProjectDetail>(`${this.apiUrl}/${id}`, request);
  }
}
