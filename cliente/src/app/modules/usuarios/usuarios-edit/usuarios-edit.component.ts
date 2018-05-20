import { Component, OnInit } from '@angular/core';
import { UsuarioNovoModel } from '../model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UsuarioService } from '../service/usuario.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-usuarios-edit',
  templateUrl: './usuarios-edit.component.html',
  styleUrls: ['./usuarios-edit.component.css']
})
export class UsuariosEditComponent implements OnInit {
  usuarioModel: UsuarioNovoModel;
  nome: string;
  descricao: string;
  options: {id:number, nome:string}[];

  usuarioForm: FormGroup;

  constructor(private userService: UsuarioService, private router : Router, private route: ActivatedRoute, private fb: FormBuilder) { }

  ngOnInit() {
    this.userService.getUsuarioById(this.route.snapshot.paramMap.get('id'))
    .subscribe(
      data => {
        this.usuarioModel = data;
      }, err => {
        console.log(err);
      });

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
      perfilUsuario: this.usuarioModel.perfil
    });
  }

}
