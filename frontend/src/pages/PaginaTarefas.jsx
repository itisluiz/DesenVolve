import { LoadingButton } from '@mui/lab';
import { Container, Paper, Stack, Typography, Dialog, DialogContent, DialogTitle, Fab, Icon, TextField, DialogActions, Button, Select, MenuItem } from '@mui/material';
import { useContext, useEffect, useState } from 'react';
import { apiObterProjeto, apiTarefas } from '../api/projeto';
import { apiCriarTarefa } from '../api/tarefa';
import { enqueueSnackbar } from 'notistack';
import { useNavigate, useParams } from 'react-router-dom';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import { DatePicker } from '@mui/x-date-pickers/DatePicker';

import CardTarefa from '../components/Cards/CardTarefa';
import { apiObterEquipe } from '../api/equipe';
import { ContexoUsuario } from '../contexts/ContextoUsuario';

const PaginaTarefas = (props) => {
	const navigate = useNavigate();
	const { codigo } = useParams(); // cod projeto
	const { usuario } = useContext(ContexoUsuario);

	const [projeto, setProjeto] = useState(null);
	const [equipe, setEquipe] = useState(null);
	const [tarefas, setTarefas] = useState([]);

	const [criarTarefaAberto, setCriarTarefaAberto] = useState(false);

	const [nomeNovaTarefa, setNomeNovaTarefa] = useState('');
	const [descricaoNovaTarefa, setDescricaoNovaTarefa] = useState('');
	const [prazoNovaTarefa, setPrazoNovaTarefa] = useState(undefined);
	const [complexidadeNovaTarefa, setComplexidadeNovaTarefa] = useState(0);
	const [codigoResponsavelNovaTarefa, setCodigoResponsavelNovaTarefa] = useState('');

	const [cargoUsuario, setCargoUsuario] = useState(null);

	function cancelarCriarTarefa()
	{ 
		setCriarTarefaAberto(false);
		setNomeNovaTarefa('');
		setDescricaoNovaTarefa('');
		setPrazoNovaTarefa(undefined);
		setComplexidadeNovaTarefa(0);
		setCodigoResponsavelNovaTarefa('');
	}

	async function atualizarDadosProjeto()
	{
		apiTarefas(codigo).then(resTarefas => {
			if (!resTarefas)
				navigate('/');
			
			setTarefas(resTarefas);
		});

		apiObterProjeto(codigo).then(resProjeto => {
			if (!resProjeto)
				navigate('/');

			apiObterEquipe(resProjeto.codigoEquipe).then(resEquipe => {
				if (!resEquipe)
					navigate('/');

				setEquipe(resEquipe);
				setCargoUsuario(resEquipe.membrosEquipe.find(membro => membro.usuario.codigo == usuario.codigo).cargo);
			});
			
			setProjeto(resProjeto);
		});
	}

	function submitCriarTarefa()
	{
		apiCriarTarefa(codigo, nomeNovaTarefa, descricaoNovaTarefa, complexidadeNovaTarefa, codigoResponsavelNovaTarefa, prazoNovaTarefa).then(res => {
			atualizarDadosProjeto();
			cancelarCriarTarefa();
		});
	}

	useEffect(() => { atualizarDadosProjeto() }, []);

	return (
		<Container sx={{ display: 'flex', mt: '1.5em' }}>
			<Stack width="100%" gap="1em">

				<div style={{display: 'flex', justifyContent: 'space-between'}}>
					<Typography variant="h3" color="primary"><b>Projeto</b> {projeto?.nome}</Typography>
					<span>
						<Fab onClick={atualizarDadosProjeto} variant="contained" color="primary" sx={{m: '0 1em'}}><Icon>refresh</Icon></Fab>
						<Fab onClick={() => setCriarTarefaAberto(true)} variant="contained" color="primary"><Icon>add</Icon></Fab>
					</span>
				</div>
				
				<Paper sx={{height: '35em', width: '100%', overflow: 'scroll'}} >
					{tarefas.map((tarefa) => (
						<CardTarefa fnAttTarefas={atualizarDadosProjeto} cargoUsuario={cargoUsuario} key={tarefa.nome} tarefa={tarefa} />
					))}
				</Paper>
			</Stack>

			<Dialog
				open={criarTarefaAberto}
				onClose={cancelarCriarTarefa}
			>
				<DialogTitle>Criar nova tarefa</DialogTitle>
				<DialogContent >
					<Typography component="p">Criando nova tarefa para o projeto <b>{projeto?.nome}</b></Typography>

					<Typography mt={'1em'} variant="caption" component="p">Nome da tarefa</Typography>
					<TextField value={nomeNovaTarefa} onChange={e => setNomeNovaTarefa(e.target.value)} placeholder='Nome' fullWidth></TextField>

					<Typography mt={'1em'} variant="caption" component="p">Descrição</Typography>
					<TextField value={descricaoNovaTarefa} onChange={e => setDescricaoNovaTarefa(e.target.value)} minRows={2} multiline placeholder='Descrição' fullWidth></TextField>

					<Typography mt={'1em'} variant="caption" component="p">Complexidade</Typography>
					<Select value={complexidadeNovaTarefa} onChange={e => setComplexidadeNovaTarefa(e.target.value)} fullWidth>
						<MenuItem value={0}>Não estabelecido</MenuItem>
						<MenuItem value={1}>Baixa</MenuItem>
						<MenuItem value={2}>Média</MenuItem>
						<MenuItem value={3}>Alta</MenuItem>
					</Select>

					<Typography mt={'1em'} variant="caption" component="p">Responsável</Typography>
					<Select value={codigoResponsavelNovaTarefa} onChange={e => setCodigoResponsavelNovaTarefa(e.target.value)} placeholder='Complexidade' fullWidth>
						{equipe && equipe.membrosEquipe.map((membro) => (
							<MenuItem key={membro.usuario.codigo} value={membro.usuario.codigo}>{membro.usuario.nome} {membro.usuario.sobrenome}</MenuItem>
						))}
					</Select>

					<Typography mt={'1em'} variant="caption" component="p">Prazo (Opcional)</Typography>
					<LocalizationProvider sx={{width: '100%'}} dateAdapter={AdapterDayjs}>
						<DatePicker onChange={v => setPrazoNovaTarefa(v)} sx={{width: '100%'}} fullWidth />
					</LocalizationProvider>

				</DialogContent>
				<DialogActions>
					<Button onClick={cancelarCriarTarefa}>Cancelar</Button>
					<Button onClick={submitCriarTarefa} disabled={
						nomeNovaTarefa.length < 2 ||
						descricaoNovaTarefa.length < 2 ||
						!codigoResponsavelNovaTarefa	
					}>Criar tarefa</Button>
				</DialogActions>
			</Dialog>

		</Container>
	);
};

export default PaginaTarefas;
