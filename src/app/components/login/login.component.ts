import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { trigger, state, style, animate, transition } from '@angular/animations';

@Component({
  selector: 'app-login',
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  animations: [
    trigger('errorAnimation', [
      state('void', style({
        opacity: 0,
        transform: 'translateY(-20px)'
      })),
      state('visible', style({
        opacity: 1,
        transform: 'translateY(0)'
      })),
      transition('void => visible', [
        animate('300ms ease-out')
      ]),
      transition('visible => void', [
        animate('200ms ease-in')
      ])
    ])
  ],
  standalone: true
})
export class LoginComponent {
  userName = '';
  password = '';
  errorMessage = '';
  showError = false;
  errorState = 'visible';

  constructor(
    private router: Router,
    private authService: AuthService
  ) { }

  onSubmit() {
    if (!this.userName || !this.password) {
      this.displayError('Por favor, preencha todos os campos');
      return;
    }

    this.authService.login({ userName: this.userName, password: this.password }).subscribe(
      (res) => {
        this.router.navigateByUrl('/');
      },
      (err) => {
        let message = 'Erro ao fazer login. Tente novamente mais tarde.';
        
        if (err.status === 401) {
          message = 'Usuário ou senha incorretos';
        } else if (err.status === 403) {
          message = 'Acesso não autorizado';
        }
        
        this.displayError(message);
      }
    );
  }

  private displayError(message: string) {
    this.errorMessage = message;
    this.showError = true;
    this.errorState = 'visible';
    
    setTimeout(() => {
      this.errorState = 'void';
      setTimeout(() => {
        this.showError = false;
      }, 200);
    }, 3000);
  }
}