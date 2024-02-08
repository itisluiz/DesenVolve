import { LoadingButton } from '@mui/lab';
import { Container, Paper, Stack, Typography, Dialog, DialogContent, DialogTitle, Fab, Icon, TextField, DialogActions, Button } from '@mui/material';
import { useEffect, useState } from 'react';
import { apiEquipes } from '../api/usuario';
import { apiCriarEquipe } from '../api/equipe';
import { enqueueSnackbar } from 'notistack';

import CardEquipe from '../components/Cards/CardEquipe';

const PaginaEquipes = (props) => {
	const [criarEquipeAberto, setCriarEquipeAberto] = useState(false);
	const [nomeNovaEquipe, setNomeNovaEquipe] = useState('');
	const [equipesUsuario, setEquipesUsuario] = useState([]);
	const [carregandoCriarEquipe, setCarregandoCriarEquipe] = useState(false);

	function cancelarCriarEquipe() { setCriarEquipeAberto(false); setNomeNovaEquipe(''); }

	async function atualizarEquipesUsuario()
	{
		apiEquipes().then(equipes => setEquipesUsuario(equipes) ?? []);
	}

	async function submitCriarEquipe()
	{
		setCarregandoCriarEquipe(true);
		apiCriarEquipe(nomeNovaEquipe).then(equipe => {
			if (equipe)
			{
				enqueueSnackbar('Equipe criada com sucesso!', { variant: 'success' });
				atualizarEquipesUsuario();
				cancelarCriarEquipe();
			}
			
			setCarregandoCriarEquipe(false);
		});
	}

	useEffect(() => { atualizarEquipesUsuario() }, []);

	return (
		<Container sx={{ display: 'flex', mt: '1.5em' }}>
			<Stack width="100%" gap="1em">

				<div style={{display: 'flex', justifyContent: 'space-between'}}>
					<Typography variant="h3" color="primary" fontWeight="bold">Suas equipes</Typography>
					<span>
						<Fab onClick={atualizarEquipesUsuario} variant="contained" color="primary" sx={{m: '0 1em'}}><Icon>refresh</Icon></Fab>
						<Fab onClick={() => setCriarEquipeAberto(true)} variant="contained" color="primary"><Icon>add</Icon></Fab>
					</span>
				</div>
				
				<Paper sx={{height: '35em', width: '100%', overflow: 'scroll'}} >
					{equipesUsuario.map((equipe) => (
						<CardEquipe key={equipe.nome} equipe={equipe} />
					))}
				</Paper>
			</Stack>
			<Dialog
				open={criarEquipeAberto}
				onClose={cancelarCriarEquipe}
			>
				<DialogTitle>Criar nova equipe</DialogTitle>
				<DialogContent>
					<TextField value={nomeNovaEquipe} onChange={(e) => setNomeNovaEquipe(e.target.value)} autoFocus placeholder="Nome da equipe" label="Nome" fullWidth variant="standard" />
				</DialogContent>
				<DialogActions>
					<Button onClick={cancelarCriarEquipe}>Cancelar</Button>
					<LoadingButton onClick={submitCriarEquipe} loading={carregandoCriarEquipe} disabled={nomeNovaEquipe.length < 2}>Criar</LoadingButton>
				</DialogActions>
			</Dialog>
		</Container>
	);
};

export default PaginaEquipes;
