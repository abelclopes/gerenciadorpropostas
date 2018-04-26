import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr'
import { User } from './../../shared/services/usuarios/user.model';
import { UserService } from './../../shared/services/usuarios/user.service';

@Component({
  selector: 'app-usuario-cadastro',
  templateUrl: './usuario-cadastro.component.html',
  styleUrls: ['./usuario-cadastro.component.css']
})
export class UsuarioCadastroComponent implements OnInit {
  user: User;
  emailPattern = "^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";

  constructor(private userService: UserService, private toastr: ToastrService) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm(form?: NgForm) {
    if (form != null)
      form.reset();
    this.user = {
      Nome: '',
      Password: '',
      Email: '',
      Police: '',
      DataNascimento: null
    }
  }

  OnSubmit(form: NgForm) {
    this.userService.registrarUsuario(form.value)
      .subscribe((data: any) => {
        if (data.Succeeded == true) {
          this.resetForm(form);
          this.toastr.success('Usuário criado com sucesso');
        }
        else
          this.toastr.error(data.Errors[0]);
      });
  }

}
