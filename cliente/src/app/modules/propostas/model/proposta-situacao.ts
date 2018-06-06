import { UsuariosClans } from "../../usuarios/model/usuario-clans.model";

export class PropSituacao{
    constructor(
        public abilitaOpcaoAprovarProposta: boolean,
        public aprovadaAnalistaFinanceiro: boolean,
        public aprovadaDiretorFinanceiro: boolean,
        public aprovar: boolean,
        public necessitaAprovavaoDiretorFinanceiro: boolean,
        public propostaHistorico: boolean,
        public status: number,
        public valorPropostaAcimaDoLimiteDesMill: boolean
    ){}
}
export class PropSituacaoResponse{
    constructor(public ok: boolean, public response: {propSituacao: PropSituacao,usuario: UsuariosClans}){}
}