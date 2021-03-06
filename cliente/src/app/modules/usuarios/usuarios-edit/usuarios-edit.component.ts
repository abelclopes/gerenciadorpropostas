import { Component, OnInit } from '@angular/core';
import { UsuarioNovoModel } from '../model';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { UsuarioService } from '../service/usuario.service';
import { Router, ActivatedRoute } from '@angular/router';
import { GeproMaskUtilService } from '../../../shared/diretivas';
import { UsuariosModel } from './../model/usuario.model';

@Component({
  selector: 'app-usuarios-edit',
  templateUrl: './usuarios-edit.component.html',
  styleUrls: ['./usuarios-edit.component.css'],
  moduleId: module.id.toString()
})
export class UsuariosEditComponent implements OnInit {
  public dataFormatBr = GeproMaskUtilService.DATE_BR_GENERATOR;
  public maskCpf = GeproMaskUtilService.CPF_MASK_GENERATOR;
  usuarioModel: UsuarioNovoModel;
  permissaoNivel: number;

  nome: string;
  descricao: string;
  options: {nivel:number, nome:string}[];

  usuarioForm: FormGroup;

  constructor(private userService: UsuarioService, private router : Router, private route: ActivatedRoute, private fb: FormBuilder) { }

  ngOnInit() {
    this.userService.getUsuarioById(this.route.snapshot.paramMap.get('id'))
    .subscribe(
      data => {
        this.usuarioModel = data;
        this.usuarioModel.dataNacimento =  this.formatData(data); 
        this.permissaoNivel = data.permissaoNivel;
        console.log(this.usuarioModel)
      }, err => {
        console.log(err);
      });

    this.userService.getPerfis().subscribe(res => this.options = res);

    this.usuarioForm = this.fb.group({
      nome: ['', Validators.required ],
      cpf: ['', Validators.required ],
      email: ['', Validators.required ],
      senha: new FormControl(null),
      perfilUsuario: ['', Validators.required ],
      dataNacimento: ['', Validators.required ],
    });
  }
  onSubmit(model){
    console.info('cadastrar editar Usuario',model)
    this.usuarioModel.nome = model.nome;
    this.usuarioModel.cpf = model.cpf.replace(/[^0-9]+/g,'');
    this.usuarioModel.email = model.email;
    this.usuarioModel.dataNacimento = model.dataNacimento.replace('/','-');
    this.usuarioModel.permissaoNivel = model.permissaoNivel;

    let usuario: UsuariosModel;
    let usuarioId: string = this.route.snapshot.paramMap.get('id');
    this.userService.updateUsuario(model, usuarioId)
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

  formatData(data): string {
    let mes = new Date(data.dataNacimento).getMonth()+1;
    let Month = ("00" + mes).slice(-2);
    let dia = new Date(data.dataNacimento).getUTCDate();
    let day = ("00" + dia).slice(-2);    
    return `${day}/${Month}/${new Date(data.dataNacimento).getUTCFullYear()}`;
  }
}
