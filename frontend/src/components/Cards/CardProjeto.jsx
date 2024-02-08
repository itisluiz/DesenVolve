import React from 'react';
import { Button, Card, CardContent, Divider, Icon, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import formatptbr from '../../util/timeagolocale';

const CardProjeto = (props) => {
	const navigate = useNavigate();
	return (
		<Card>
			<CardContent sx={{display: 'flex'}}>
				<div style={{width: '100%'}}>
					<Typography variant="h5" component="h2" sx={{mb: '0.3em'}}><Icon color='primary' sx={{mr: '0.35em', mb: '-0.2em'}}>hive</Icon>{props.projeto.nome}</Typography>
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