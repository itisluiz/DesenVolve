import { Box } from '@mui/material';
import { SnackbarProvider } from 'notistack';
import Cabecalho from './Cabecalho';
import Rodape from './Rodape';
import Tema from './Tema';
import './../styles/Layout.css'

const Layout = (props) => {
	return (
		<Tema>
			<SnackbarProvider>
				<Box display="flex" flexDirection="column" height="100vh">
					<Cabecalho />
						<Box flex="1 0">
							{props.children}
						</Box>
					<Rodape />
				</Box>
			</SnackbarProvider>
		</Tema>
	);
};

export default Layout;
