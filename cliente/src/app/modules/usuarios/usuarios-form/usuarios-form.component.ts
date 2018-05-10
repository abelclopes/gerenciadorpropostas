import { Component, OnInit } from '@angular/core';
import { UsuarioService } from '../service/usuario.service';
import { Router } from '@angular/router';
import { UsuariosNovoModel, UsuarioNovoModel } from '../model';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-usuarios-form',
  templateUrl: './usuarios-form.component.html',
  styleUrls: ['./usuarios-form.component.css']
})
export class UsuariosFormComponent implements OnInit {
  usuarioModel: UsuariosNovoModel;
  nome: string;
  descricao: string;
  options: {id:number, nome:string}[];

  usuarioForm: FormGroup;

  constructor(private userService: UsuarioService,private router : Router, private fb: FormBuilder) { }

  ngOnInit() {
    this.userService.getPerfis().subscribe(res => this.options = res);

    this.usuarioForm = this.fb.group({
      nome: ['', Validators.required ],
      cpf: ['', Validators.required ],
      email: ['', Validators.required ],
      senha: ['', Validators.required ],
      perfilUsuario: ['', Validators.required ],
      dataNacimento: ['', Validators.required ],
    });
  }
  onSubmit(model){

    let novaUsuario: UsuarioNovoModel;
    this.userService.novoUsuario(novaUsuario)
      .subscribe(
        data => {
          if(data['ok'] == true)
            this.router.navigate(['/categorias']);
        }, err => {
          console.log(err);
        }
      );
  }
  resetForm(){
    this.usuarioForm.reset({
      name: this.usuarioModel.nome,
      email: this.usuarioModel.email,
      senha: this.usuarioModel.senha,
      cpf: this.usuarioModel.cpf,
      dataNacimento: this.usuarioModel.dataNacimento,
      perfilUsuario: this.usuarioModel.perfilUsuario
    });
  }

}
