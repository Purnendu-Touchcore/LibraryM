import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Record } from './record';
import { environment } from 'src/environments/environment';

@Injectable()
export class RecordService {
  recordsUrl = environment.baseUrl+'/record';  // URL to web api

  constructor(private http: HttpClient) {}

  getRecords(): Observable<Record[]> {
    return this.http.get<Record[]>(this.recordsUrl)
  }

  getUserRecordsByPage(bookStatus: string, email: string, pageSize: number, pageNumber:number): Observable<Record[]> {
    return this.http.get<Record[]>(`${this.recordsUrl}/${bookStatus}/${email}/${pageSize}/${pageNumber}`)
  }

  getRecordsByPage(bookStatus: string, pageSize: number, pageNumber:number): Observable<Record[]> {
    let email = "All"
    return this.http.get<Record[]>(`${this.recordsUrl}/${bookStatus}/${email}/${pageSize}/${pageNumber}`)
  }

  addRecord(record: Record): Observable<number> {
    return this.http.post<number>(this.recordsUrl, record)
  }

  deleteRecord(id: number): Observable<unknown> {
    const url = `${this.recordsUrl}/${id}`; // DELETE api/records/42
    return this.http.delete(url)
  }

  updateRecord(record: Record): Observable<unknown> {      
    return this.http.put(`${this.recordsUrl}/${record.id}`, record)
  }

  getStatus(userId: number): Observable<Record[]>{
    return this.http.get<Record[]>(`${this.recordsUrl}/book/${userId}`)
  }
}
