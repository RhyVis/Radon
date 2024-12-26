<script setup lang="ts">
import { usePdxStore } from "@/pages/with/pdx-parser/scripts/store.ts";
import { useGlobalStore } from "@/store/global.ts";
import { useToggle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { AddIcon, DeleteIcon, DownloadIcon, SettingIcon, UploadIcon } from "tdesign-icons-vue-next";
import { useI18n } from "vue-i18n";

const store = usePdxStore();
const { t } = useI18n();
const { replacer, addReplacerKey, addReplacerValue } = storeToRefs(store);
const { authPassed } = storeToRefs(useGlobalStore());
const [menuReplaceAddVisible, setMenuReplaceAddVisible] = useToggle(false);

const visible = defineModel("visible", { required: true, default: false });
</script>

<template>
  <t-dialog v-model:visible="visible" :header="t('replace.title')" width="85%">
    <div v-if="!menuReplaceAddVisible">
      <t-list :split="true" :stripe="true" size="small" style="max-height: 280px">
        <t-list-item v-for="(value, key) in replacer" :key="key">
          <t-list-item-meta :description="value" :title="key.toString()" />
          <template #action>
            <t-button
              shape="circle"
              size="small"
              theme="warning"
              variant="outline"
              @click="store.removeReplacer(key.toString())"
            >
              <DeleteIcon />
            </t-button>
          </template>
        </t-list-item>
      </t-list>
    </div>
    <div v-else>
      <t-form label-align="top">
        <t-form-item :label="t('replace.key')">
          <t-input v-model="addReplacerKey" />
        </t-form-item>
        <t-form-item :label="t('replace.value')">
          <t-input v-model="addReplacerValue" />
        </t-form-item>
        <t-form-item :label="authPassed ? t('replace.modify') : t('replace.modifyOnly')">
          <t-space :size="6">
            <t-button shape="circle" @click="store.updateReplacer()">
              <AddIcon />
            </t-button>
            <t-button v-if="authPassed" shape="circle" @click="store.setSyncReplacer()">
              <UploadIcon />
            </t-button>
            <t-button v-if="authPassed" shape="circle" @click="store.getSyncReplacer()">
              <DownloadIcon />
            </t-button>
          </t-space>
        </t-form-item>
      </t-form>
    </div>
    <template #footer>
      <t-button shape="circle" theme="default" @click="setMenuReplaceAddVisible()">
        <SettingIcon />
      </t-button>
    </template>
  </t-dialog>
</template>

<i18n locale="en">
replace:
  title: Replace Value
  key: Key
  value: Value
  modify: Modify / Upload / Read
  modifyOnly: Modify
  sync: Sync
  download: Download
  upload: Upload
  tips: The key is not case sensitive
</i18n>

<i18n locale="zh-CN">
replace:
  title: 替换值
  key: 键
  value: 值
  modify: 修改 / 上传 / 读取
  modifyOnly: 修改
  sync: 同步
  download: 下载
  upload: 上传
  tips: 键不区分大小写
</i18n>
