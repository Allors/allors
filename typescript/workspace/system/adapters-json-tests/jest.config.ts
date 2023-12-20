/* eslint-disable */
export default {
  displayName: 'workspace-system-adapters-json-tests',
  preset: '../../../../jest.preset.js',
  testEnvironment: 'node',
  transform: {
    '^.+\\.[tj]s$': ['ts-jest', { tsconfig: '<rootDir>/tsconfig.spec.json' }],
  },
  moduleFileExtensions: ['ts', 'js', 'html'],
  coverageDirectory:
    '../../../../coverage/libs/workspace/system/adapters-json-tests',
  // Allors
  reporters: [
    'default',
    [
      'jest-trx-results-processor',
      {
        outputFile:
          '../artifacts/tests/typscript.workspace-adapters-json-system.trx',
      },
    ],
  ],
  maxWorkers: 1,
  testTimeout: 60000 * 10,
};
