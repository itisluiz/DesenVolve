import { Paper, Stack, Typography, Icon } from '@mui/material';

const Rodape = () => {
	return (
		<Paper component="footer" width="100%">
				<Stack m="0.75em" alignItems="center">
					{import.meta.env.DEV &&
						<>
							<Typography>Versão <b>{import.meta.env.VITE_REACT_APP_VERSION}</b></Typography>
							<Typography color='primary' fontWeight='bold'><Icon sx={{mb: '-0.25em', mr: '0.2em'}}>code</Icon>Desenvolvimento</Typography>
						</>
					}
					<Typography variant="caption"><b>Projeto Volvo</b> Luiz Krüger e João Wozniack</Typography>
					<Typography variant="caption">© 2024 DesenVolve. Nenhum direito reservado.</Typography>
				</Stack>
		</Paper>
	);
};

export default Rodape;
