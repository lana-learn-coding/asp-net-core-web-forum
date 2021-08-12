module.exports = {
  root: true,
  env: {
    node: true,
  },
  extends: [
    'plugin:vue/essential',
    '@vue/airbnb',
    '@vue/typescript/recommended',
  ],
  parserOptions: {
    ecmaVersion: 2020,
  },
  rules: {
    'no-console': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
    'no-debugger': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
    'max-len': [
      'warn',
      { code: 150 },
    ],
    'class-methods-use-this': 'off',
    'no-shadow': 'off',
    'no-param-reassign': ['error', { props: false }],
    'object-curly-newline': ['error', {
      ImportDeclaration: {
        multiline: true,
        minProperties: 8,
      },
      ExportDeclaration: {
        multiline: true,
        minProperties: 8,
      },
    }],
  },
};
