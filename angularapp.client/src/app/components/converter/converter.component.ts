import { Component, OnInit } from '@angular/core';
import { CurrencyService } from '../../services/currency.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-converter',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './converter.component.html',
  styleUrls: ['./converter.component.css']
})
export class ConverterComponent implements OnInit {

  amount: number = 1;
  fromCurrency: string = 'USD';
  toCurrency: string = 'EUR';
  convertedAmount: number | null = null;
  currencies: string[] = [];

  constructor(private currencyService: CurrencyService) { }

  ngOnInit() {
    this.currencyService.getRates().subscribe(data => {
      console.log('Получено от бэка:', data);
      this.currencies = Object.keys(data.rates);
    });
  }

  convert() {
    if (this.amount && this.fromCurrency && this.toCurrency) {
      this.currencyService.convertAmount(this.amount, this.fromCurrency, this.toCurrency)
        .subscribe(data => {
          console.log('Результат конвертации:', data);
          this.convertedAmount = data.result; // 👉 Берём result из JSON-ответа
        });
    }
  }
}
