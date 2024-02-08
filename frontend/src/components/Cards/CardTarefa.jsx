import React, { useContext, useState } from 'react';
import { Avatar, Card, CardContent, Divider, Icon, Typography } from '@mui/material';
import formatptbr from '../../util/timeagolocale';
import { ContexoUsuario } from '../../contexts/ContextoUsuario';
import { apiFinalizarTarefa, apiIniciarTarefa } from '../../api/tarefa';
import { LoadingButton } from '@mui/lab';

const CardTarefa = (props) => {
	const { usuario } = useContext(ContexoUsuario);
	let corComplexidade = undefined;

	const [carregandoEstado, setCarregandoEstado] = useState(false);

	if (props.tarefa.complexidade == 1)
		corComplexidade = '#008F39';
	else if (props.tarefa.complexidade == 2)
		corComplexidade = '#FFA500';
	else if (props.tarefa.complexidade == 3)
		corComplexidade = '#FF0000';
	else
		corComplexidade = '#AAAAAA';

	function temPermissaoSobre()
	{
		if (props.tarefa.responsavel.codigo == usuario.codigo)
			return true;
		
		return props.cargoUsuario?.codigo >= 1;
	}

	function submitIniciarTarefa()
	{
		setCarregandoEstado(true);
		apiIniciarTarefa(props.tarefa.codigo).then(() => {
			props.fnAttTarefas();
			setCarregandoEstado(false);
		});
	}

	function submitFinalizarTarefa()
	{
		setCarregandoEstado(true);
		apiFinalizarTarefa(props.tarefa.codigo).then(() => {
			props.fnAttTarefas();
			setCarregandoEstado(false);
		});
	}

	function obterBotaoAcao()
	{
		if (temPermissaoSobre())
		{
			if (!props.tarefa.inicio)
				return <LoadingButton loading={carregandoEstado} onClick={submitIniciarTarefa} color='success' sx={{ml: 'auto'}} variant='contained'>INICIAR AGORA</LoadingButton>;
			else if (!props.tarefa.finalizado)
				return <LoadingButton loading={carregandoEstado} onClick={submitFinalizarTarefa} color='error' sx={{ml: 'auto'}} variant='contained'>FINALIZAR AGORA</LoadingButton>;
		}

		if (props.tarefa.inicio && props.tarefa.finalizado)
			return <Icon color='success' sx={{ml: 'auto'}}>done</Icon>;
		else if (props.tarefa.inicio)
			return <Icon color='warning' sx={{ml: 'auto'}}>timer</Icon>;

		return <Icon sx={{ml: 'auto', opacity: 0.5}}>pending</Icon>;
	}

	return (
		<Card>
			<CardContent sx={{display: 'flex'}}>
				<div style={{width: "100%"}}>
					<span style={{display: 'flex'}}>
						<Typography variant="h5" component="h2"><Icon color='primary' sx={{mr: '0.35em', mb: '-0.2em'}}>task_alt</Icon>{props.tarefa.nome}</Typography>
						{obterBotaoAcao()}
					</span>

					<span style={{display: 'flex'}}>
						<Typography color="primary" mr="0.3em" fontWeight="bold">Responsável:</Typography>
						<Avatar sx={{ width: '1em', height: '1em', mr: '0.3em' }} src={props.tarefa.responsavel.urlAvatar} />
						<Typography component="h2">{props.tarefa.responsavel.nomeDisplay}</Typography>
					</span>

					<span style={{display: 'flex'}}>
						<Typography mr="0.3em" fontWeight="bold">Complexidade:</Typography>
						<Typography sx={{mt: '0.15em', height: '1.5em', backgroundColor: corComplexidade, p: '0em 0.4em 0em 0.4em', borderRadius: '0.5em', opacity: 0.7, fontSize: '0.8em'}} color='white' fontWeight="bold">{props.tarefa.complexidadeNome}</Typography>
					</span>

					<span style={{display: 'flex'}}>
						<Typography mr="0.3em" fontWeight="bold">Descrição:</Typography>
						<Typography color="textSecondary">{props.tarefa.descricao}</Typography>
					</span>

					<Divider sx={{mt: '1em', mb: '0.2em'}} variant='fullWidth' />

					<div style={{width: '100%'}}>
						<Typography>
							<Icon color='success' sx={{mr: '0.35em', mb: '-0.1em', fontSize: '0.9em'}}>calendar_today</Icon>
							Início: <Typography variant='caption'>{!props.tarefa.inicio ? 'Não iniciado' : formatptbr(props.tarefa.inicio) }</Typography> 
						</Typography>
						<Typography >
							<Icon color='error' sx={{mr: '0.35em', mb: '-0.1em', fontSize: '0.9em'}}>event_available</Icon>
							Término: <Typography variant='caption'>{!props.tarefa.finalizado ? 'Não finalizado' : formatptbr(props.tarefa.finalizado) }</Typography> 
						</Typography>
						<Typography>
							<Icon color='warning' sx={{mr: '0.35em', mb: '-0.1em', fontSize: '0.9em'}}>event_available</Icon>
							Prazo: <Typography variant='caption'>{!props.tarefa.prazo ? 'Sem prazo estipulado' : formatptbr(props.tarefa.prazo) }</Typography> 
						</Typography>
					</div>
				</div>
			</CardContent>
		</Card>
	);
};

export default CardTarefa;