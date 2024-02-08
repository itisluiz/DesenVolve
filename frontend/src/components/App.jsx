import { HashRouter, Route, Routes } from 'react-router-dom'
import Contexo from './Contexto';
import Layout from './Layout';
import RequerLogin from './RequerLogin';

import PaginaAutenticar from '../pages/PaginaAutenticar';
import PaginaEquipes from '../pages/PaginaEquipes';
import PaginaProjetos from '../pages/PaginaProjetos';
import PaginaTarefas from '../pages/PaginaTarefas';

function App() {
	return (
		<Contexo>
			<Layout>
				<HashRouter>
					<Routes>
						<Route index path="/" element={<RequerLogin><PaginaEquipes /></RequerLogin>} />
						<Route path="/equipe/:codigo" element={<RequerLogin><PaginaProjetos /></RequerLogin>} />
						<Route path="/projeto/:codigo" element={<RequerLogin><PaginaTarefas /></RequerLogin>} />
						<Route path="/autenticar" element={<PaginaAutenticar />} />
					</Routes>
				</HashRouter>
			</Layout>
		</Contexo>
	)
}

export default App
