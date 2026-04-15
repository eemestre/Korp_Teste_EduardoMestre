import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cadastro',
  standalone: true,
  templateUrl: './cadastro.html',
  styleUrls: ['./cadastro.css'],
  imports: [CommonModule],
})
export class Cadastro {
  constructor(private http: HttpClient) {}

  descricao = '';
  saldo = 0;

  carregando = false;

  setDescricao(event: Event) {
    const input = event.target as HTMLInputElement;
    this.descricao = input.value;
  }

  setSaldo(event: Event) {
    const input = event.target as HTMLInputElement;
    this.saldo = Number(input.value);
  }

  async submit(event: Event) {
    event.preventDefault();

    this.carregando = true;

    try {
      await firstValueFrom(
        this.http.post('http://localhost:5095/produtos', {
          descricao: this.descricao,
          saldo: this.saldo,
        }),
      );

      this.descricao = '';
      this.saldo = 0;

      alert('Produto cadastrado!');
    } catch (e: any) {
      alert(e.message);
      console.log(e);
    }

    this.carregando = false;
  }
}
