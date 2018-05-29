import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { UsuarioService } from '../service/usuario.service';
import { UsuarioNovoModel } from '../model';
import { GeproMaskUtilService } from '../../../shared/diretivas';

@Component({
  selector: 'app-usuarios-form',
  templateUrl: './usuarios-form.component.html',
  styleUrls: ['./usuarios-form.component.css']
})
export class UsuariosFormComponent implements OnInit {
  public dataFormatBr = GeproMaskUtilService.DATE_BR_GENERATOR;
  public maskCpf = GeproMaskUtilService.CPF_MASK_GENERATOR;
  usuarioModel: UsuarioNovoModel;
  nome: string;
  descricao: string;
  options: {nivel:number, nome:string}[];

  usuarioForm: FormGroup;

  constructor(private userService: UsuarioService, private router : Router, private fb: FormBuilder) { }

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
    model.cpf = model.cpf.replace('.','').replace('-','');
    model.dataNacimento = model.dataNacimento.replace('/','-');
    this.userService.novoUsuario(model)
      .subscribe(
        data => {
          if(data['ok'] == true)
            this.router.navigate(['/usuarios']);
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
      perfilUsuario: this.usuarioModel.permissaoNivel
    });
  }

}
