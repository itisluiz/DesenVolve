import React from 'react';
import { Button, Card, CardContent, Divider, Icon, LinearProgress, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import formatptbr from '../../util/timeagolocale';

const CardProjeto = (props) => {
	const navigate = useNavigate();

	
	function insightTarefas() {

		if (!props.projeto.qntTarefas)
			return <Typography variant='caption'>Este projeto não possuí tarefas cadastradas</Typography>
		else
		{
			let porcentagem = (props.projeto.qntTarefasFinalizadas / props.projeto.qntTarefas) * 100;

			let corInsight = undefined;

			if (props.projeto.qntTarefasFinalizadas != props.projeto.qntTarefas)
				corInsight = "warning";
			else
				corInsight = "success";
			
			return (
				<div>
					<Typography>
						<Icon color={corInsight} sx={{mr: '0.35em', mb: '-0.2em'}}>task_alt</Icon>
						{props.projeto.qntTarefasFinalizadas} de {props.projeto.qntTarefas} tarefas finalizadas
					</Typography>
					<LinearProgress sx={{m: '0.5em 0'}} color={corInsight} variant='determinate' value={porcentagem}/>
				</div>
			);
		}
	}

	return (
		<Card>
			<CardContent sx={{display: 'flex'}}>
				<div style={{width: '100%'}}>
					<Typography variant="h5" component="h2" sx={{mb: '0.3em'}}><Icon color='primary' sx={{mr: '0.35em', mb: '-0.2em'}}>inventory_2</Icon>{props.projeto.nome}</Typography>
					{insightTarefas()}
					<Divider sx={{mb: '0.5em', mt: '0.2em'}} variant='fullWidth' />
					<Typography>
						<Icon color='success' sx={{mr: '0.35em', mb: '-0.1em', fontSize: '0.9em'}}>calendar_today</Icon>
						Início: <Typography variant='caption'>{!props.projeto.inicio ? 'Não iniciado' : formatptbr(props.projeto.inicio) }</Typography> 
					</Typography>
					<Typography >
						<Icon color='error' sx={{mr: '0.35em', mb: '-0.1em', fontSize: '0.9em'}}>event_available</Icon>
						Término: <Typography variant='caption'>{!props.projeto.finalizado ? 'Não finalizado' : formatptbr(props.projeto.finalizado) }</Typography> 
					</Typography>
					<Typography>
						<Icon color='warning' sx={{mr: '0.35em', mb: '-0.1em', fontSize: '0.9em'}}>event_available</Icon>
						Prazo: <Typography variant='caption'>{!props.projeto.prazo ? 'Sem prazo estipulado' : formatptbr(props.projeto.prazo) }</Typography> 
					</Typography>
				</div>
				
				<Button onClick={() => navigate(`/projeto/${props.projeto.codigo}`)} sx={{ marginLeft: 'auto', minWidth: '15em' }}>
					Acessar página
					<Icon sx={{ marginLeft: '0.35em' }}>arrow_forward</Icon>
				</Button>
			</CardContent>
		</Card>
	);
};

export default CardProjeto;