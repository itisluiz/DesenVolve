import { HashRouter, Route, Routes } from 'react-router-dom'
import Contexo from './Contexto';
import Layout from './Layout';
import RequerLogin from './RequerLogin';

import PaginaHome from '../pages/PaginaHome';
import PaginaAutenticar from '../pages/PaginaAutenticar';

function App() {
	return (
		<Contexo>
			<Layout>
				<HashRouter>
					<Routes>
						<Route index path="/" element={<RequerLogin><PaginaHome /></RequerLogin>} />
						<Route path="/autenticar" element={<PaginaAutenticar />} />
					</Routes>
				</HashRouter>
			</Layout>
		</Contexo>
	)
}

export default App
