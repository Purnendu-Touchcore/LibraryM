import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Record } from './record';

@Injectable()
export class RecordService {
  recordsUrl = 'https://librarym-purnendu.azure-api.net/record';  // URL to web api

  constructor(private http: HttpClient) {}

  getRecords(): Observable<Record[]> {
    return this.http.get<Record[]>(this.recordsUrl)
  }

  getRecordsByPage(pageSize: number, currentPage:number): Observable<Record[]> {
    return this.http.get<Record[]>(`${this.recordsUrl}/page/${pageSize}/${currentPage}`)
  }

  getUserRecords(id: any): Observable<Record[]> {
    return this.http.get<Record[]>(`${this.recordsUrl}/${id}`)
  }

  // searchRecords(id: string): Observable<Record> {
  //   id = id.trim();

  //   const url = `${this.recordsUrl}/${id}`;
  //   console.log(url)
  //   return this.http.get<Record>(url)
  // }


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
