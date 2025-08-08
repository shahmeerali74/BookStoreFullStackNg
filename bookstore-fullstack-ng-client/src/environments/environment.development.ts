const hostname = window.location.hostname;

export const environment = {
  baseUrl: hostname === 'localhost'
  ? 'http://localhost:5001':
    'http://135.222.42.11:5001'
};
