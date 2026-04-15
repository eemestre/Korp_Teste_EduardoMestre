import { Routes } from '@angular/router';
import { Home } from './components/home/home';
import { Cadastro } from './components/cadastro/cadastro';
import { Emitir } from './components/emitir/emitir';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'cadastrar-produto', component: Cadastro },
  { path: 'emitir-nota', component: Emitir },
];
