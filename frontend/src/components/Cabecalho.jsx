import { AppBar, Toolbar, Typography, Stack, Icon, Avatar, Menu, MenuItem, ListItemIcon, ListItemText } from '@mui/material';
import { useContext, useState } from 'react';
import { ContexoUsuario } from '../contexts/ContextoUsuario';
import { logout } from '../api/usuario';

const Cabecalho = () => {
	const { usuario, setUsuario } = useContext(ContexoUsuario);
	const [ancoraMenuUsuario, setAncoraMenuUsuario] = useState(null);

	function submitLogout() { logout().then(() => setUsuario(null)); }

	return (
		<AppBar position="sticky">
			<Toolbar>
				<Icon sx={{fontSize: '1.75em'}}>checklist</Icon>
				<Stack marginLeft="0.75em" spacing="-0.75em" flexGrow="1">
					<Typography fontWeight="bolder" variant="h6">DesenVolve</Typography>
					<Typography variant="caption">Sistema de gerenciamento e apoio para projetos</Typography>
				</Stack>
				{usuario && <>
					<div onClick={e => setAncoraMenuUsuario(e.currentTarget)} style={{ cursor: 'pointer', display: 'flex', alignItems: 'center' }}>
						<Typography sx={{ mr: '0.5em', fontSize: '1.1em', fontWeight: 'bold' }}>{usuario.nomeDisplay}</Typography>
						<Avatar src={usuario.urlAvatar} sx={{ cursor: 'pointer' }} />
					</div>
					<Menu anchorEl={ancoraMenuUsuario} open={Boolean(ancoraMenuUsuario)} onClose={() => setAncoraMenuUsuario(null)}>
						<MenuItem onClick={() => { submitLogout(); setAncoraMenuUsuario(null); } }>
							<ListItemIcon><Icon>logout</Icon></ListItemIcon>
							<ListItemText>Sair</ListItemText>
						</MenuItem>
					</Menu>
				</>}
			</Toolbar>
		</AppBar>
	);
};

export default Cabecalho;
