/* eslint-disable */
export default {
  displayName: 'system-workspace-meta-json-tests',
  preset: '../../../../jest.preset.js',
  testEnvironment: 'node',
  transform: {
    '^.+\\.[tj]s$': ['ts-jest', { tsconfig: '<rootDir>/tsconfig.spec.json' }],
  },
  moduleFileExtensions: ['ts', 'js', 'html'],
  coverageDirectory:
    '../../../../coverage/libs/system/workspace/meta-json-tests',
  // Allors
  reporters: [
    'default',
    [
      'jest-trx-results-processor',
      {
        outputFile:
          '../artifacts/tests/typscript.system-workspace-meta-json.trx',
      },
    ],
  ],
};
