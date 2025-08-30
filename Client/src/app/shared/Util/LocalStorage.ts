const StorageKeys = {
  ACCESS_TOKEN: 'ACCESS_TOKEN',
  REFRESH_TOKEN: 'REFRESH_TOKEN',
} as const;

export type StorageKeysType = (typeof StorageKeys)[keyof typeof StorageKeys];

class StorageUtility {
  static setItem<T>(key: StorageKeysType, value: T): void {
    try {
      const jsonValue = JSON.stringify(value);
      localStorage.setItem(key, jsonValue);
    } catch (e) {}
  }

  static getItem<T>(key: StorageKeysType): T | null {
    try {
      const jsonValue = localStorage.getItem(key);
      const value = jsonValue != null ? JSON.parse(jsonValue) : null;
      return value;
    } catch (e) {
      return null;
    }
  }

  static removeItem(key: StorageKeysType): void {
    try {
      localStorage.removeItem(key);
    } catch (e) {}
  }

  static clear(): void {
    try {
      localStorage.clear();
    } catch (error) {}
  }

  static getMultipleItems(
    keys: Array<StorageKeysType>
  ): Record<StorageKeysType, any> | undefined {
    try {
      const result = localStorage['multiGet'](keys);
      const final = result.reduce((pre: any, curr: any[]) => {
        const val = curr[1] ? JSON.parse(curr[1]) : null;
        return {
          ...pre,
          [curr[0]]: val,
        };
      }, {} as Record<StorageKeysType, any>);
      return final;
    } catch (err) {
      return undefined;
    }
  }
}

export { StorageUtility, StorageKeys };
