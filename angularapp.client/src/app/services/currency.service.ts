import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CurrencyService {

  private apiUrl = 'http://localhost:5068/api/Currency';

  constructor(private http: HttpClient) { }

  getRates(): Observable<any> {
    return this.http.get(`${this.apiUrl}/rates`);
  }

  getRate(from: string, to: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/rate`, {
      params: {
        from: from,
        to: to
      }
    });
  }

  convertAmount(amount: number, from: string, to: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/convert`, {
      params: {
        amount: amount,
        from: from,
        to: to
      }
    });
  }
}
