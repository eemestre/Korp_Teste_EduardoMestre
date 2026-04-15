import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

type Produto = {
  codigo: string;
  descricao: string;
  saldo: number;
};

type NFItem = {
  codigo: string;
  quantidade: number;
};

@Component({
  selector: 'app-emitir',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './emitir.html',
  styleUrls: ['./emitir.css'],
})
export class Emitir implements OnInit {
  constructor(private http: HttpClient) {}

  produtos: Produto[] = [];
  quantidades: number[] = [];

  loading = false;
  erro = false;
  sucesso = false;

  ngOnInit() {
    this.loading = true;
    this.erro = false;

    this.http.get<Produto[]>('http://localhost:5095/produtos').subscribe({
      next: (res) => {
        console.log('Produtos recebidos:', res);

        this.produtos = res;
        this.quantidades = new Array(res.length).fill(0);

        this.loading = false;
      },
      error: (e) => {
        console.log('Erro ao buscar produtos:', e);
        this.erro = true;
        this.loading = false;
      },
    });
  }

  setQuantidade(event: Event, index: number) {
    const input = event.target as HTMLInputElement;
    const valor = Number(input.value);

    this.quantidades[index] = valor > 0 ? valor : 0;
  }

  enviarNota() {
    this.loading = true;
    this.erro = false;
    this.sucesso = false;

    const itens: NFItem[] = this.produtos
      .map((p, i) => ({
        codigo: p.codigo,
        quantidade: this.quantidades[i] || 0,
      }))
      .filter((item) => item.quantidade > 0);

    if (itens.length === 0) {
      console.log('Nenhum produto selecionado');
      this.erro = true;
      this.loading = false;
      return;
    }

    this.http
      .post('http://localhost:5219/notas', {
        produtos: itens,
      })
      .subscribe({
        next: () => {
          this.sucesso = true;
          this.quantidades = new Array(this.produtos.length).fill(0);
          this.loading = false;
        },
        error: (e) => {
          console.log('Erro ao enviar nota:', e);
          this.erro = true;
          this.loading = false;
        },
      });
  }
}
