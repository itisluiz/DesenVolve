import { Container, TextField, Stack, Paper, Tab, Box, Typography } from '@mui/material';
import { LoadingButton, TabContext, TabList, TabPanel } from '@mui/lab';
import { useState, useContext, useEffect } from 'react';
import { postAPI, getAPI } from '../api/api';
import { ContextoUsuario } from '../contexts/ContextoUsuario';
import { useNavigate } from "react-router-dom";

const PaginaHome = (props) => {
	const navigate = useNavigate();
	const {setUsuario} = useContext(ContextoUsuario);
	const [aba, setAba] = useState('1');
	const [nome, setNome] = useState('');
	const [sobrenome, setSobrenome] = useState('');
	const [email, setEmail] = useState('');
	const [senha, setSenha] = useState('');
	const [senhaRepetida, setSenhaRepetida] = useState('');

	const [carregando, setCarregando] = useState(false);

	function atualizarUsuario() {
		getAPI('usuario').then((res) => {
			if (res.ok && res.resposta)
			{
				setUsuario(res.resposta);
				navigate('/');
			}
			else
				setUsuario(null);
		});
	}

	useEffect(() => {
		atualizarUsuario();
	}, []);

	function fazerLogin() {
		setCarregando(true);
		postAPI('login', null, { "email": email, "senha": senha }).then((res) => {
			if (res.ok)
			{
				atualizarUsuario();
				setCarregando(false);
			}
			else
				setCarregando(false);
		});
	}

	function fazerCadastro() {
		setCarregando(true);
		postAPI('usuario', null, { "nome": nome, "sobrenome": sobrenome, "email": email, "senha": senha }).then((res) => {
			if (res.ok)
			{
				atualizarUsuario();
				setCarregando(false);
			}
			else
				setCarregando(false);
		});
	}

	return (
		<Container sx={{ display: 'flex', justifyContent: 'center', mt: '4em' }}>
			<Paper sx={{ width: '60%', display: 'flex', justifyContent: 'center' }}>
				<TabContext value={aba}>
					<Stack sx={{ width: '50%' }} marginY={'1em'} direction={'column'} justifyContent={'center'} alignItems={'center'} spacing={'1em'}>
						<TabList variant='fullWidth' sx={{width: '100%'}} onChange={(event, newValue) => setAba(newValue)}>
							<Tab label='Login' value={'1'} />
							<Tab label='Inscreva-se' value={'2'} />
						</TabList>

						<TabPanel value={'1'}>
							<Stack sx={{width: '25em', mb:'2em'}} direction={'column'} justifyContent={'center'} alignItems={'center'} spacing={'1em'}>
								<Typography variant='h6' color='primary'>Fazer Log-in</Typography>
								<Typography variant='body2'>Entre com sua conta DesenVolve</Typography>

								<TextField value={email} onChange={(e) => setEmail(e.target.value)} label='E-mail' variant='outlined' fullWidth />
								<TextField value={senha} onChange={(e) => setSenha(e.target.value)} label='Senha' variant='outlined' fullWidth type='password' />
								<LoadingButton loading={carregando} onClick={fazerLogin} variant='contained' disabled={email.length == 0 || senha.length == 0} fullWidth>Login</LoadingButton>
							</Stack>
						</TabPanel>

						<TabPanel value={'2'}>
							<Stack sx={{width: '25em', mb:'2em'}} direction={'column'} justifyContent={'center'} alignItems={'center'} spacing={'1em'}>
								<Typography variant='h6' color='primary'>Fazer Cadastro</Typography>
								<Typography variant='body2'>Crie uma conta DesenVolve</Typography>
								<TextField label='Nome' variant='outlined' fullWidth value={nome} onChange={(e) => setNome(e.target.value)} />
								<TextField label='Sobrenome' variant='outlined' fullWidth value={sobrenome} onChange={(e) => setSobrenome(e.target.value)} />
								<TextField label='E-mail' variant='outlined' fullWidth value={email} onChange={(e) => setEmail(e.target.value)} />
								<Typography alignSelf='self-start' variant='caption' color='error' sx={{display: senha.length > 0 && senha.length < 6 ? 'block' : 'none'}}>A senha deve ter no mínimo 6 caracteres</Typography>
								<TextField label='Senha' variant='outlined' fullWidth type='password' value={senha} onChange={(e) => setSenha(e.target.value)} error={senha.length > 0 && senha.length < 6} />
								<Typography alignSelf='self-start' variant='caption' color='error' sx={{display: senhaRepetida.length > 0 && senha != senhaRepetida ? 'block' : 'none'}}>As senhas não coincidem</Typography>
								<TextField label='Repita a senha' variant='outlined' fullWidth type='password' value={senhaRepetida} onChange={(e) => setSenhaRepetida(e.target.value)} error={senhaRepetida.length > 0 && senha != senhaRepetida} />
								<LoadingButton onClick={fazerCadastro} loading={carregando} variant='contained' disabled={nome.length < 2 || sobrenome.length < 2 || email.length < 2 || senha.length < 6 || senha != senhaRepetida } fullWidth>Cadastrar</LoadingButton>
							</Stack>
						</TabPanel>
				</Stack>
				</TabContext>
			</Paper>
		</Container>
	);
};

export default PaginaHome;
