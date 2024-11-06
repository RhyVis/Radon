<script lang="ts" setup>
import ButtonClear from "@/components/btn/ButtonClear.vue";
import ButtonCopy from "@/components/btn/ButtonCopy.vue";
import ButtonRead from "@/components/btn/ButtonRead.vue";
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import { apiPost } from "@/lib/util/apiMethods";
import type { SaveEntry } from "@/pages/data/save/scripts/entryType";
import { useSaveStore } from "@/pages/data/save/scripts/store";
import { DownloadIcon, UploadIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";

const saveStore = useSaveStore();
const { t } = useI18n();

const query = reactive({
  id: 0,
  text: "",
  note: "",
});
const result = reactive({
  text: "",
  note: "",
  sign: "---",
});
const loading = ref(false);

const handleStore = async () => {
  result.sign = "---";
  if (query.text.length === 0) {
    await MessagePlugin.warning("请勿存储空置");
  } else if (query.id < 0) {
    query.id = 0;
    await MessagePlugin.warning("ID不能小于0");
  } else {
    loading.value = true;
    try {
      const dt = (await apiPost("/api/update", query)).data;
      if (dt === 0) {
        result.sign = "存储成功";
        saveStore.update(query.id);
        await MessagePlugin.success("存储成功");
      } else {
        console.error(dt);
        result.sign = "存储失败";
        await MessagePlugin.error("存储失败");
      }
    } catch (e) {
      console.error(e);
      result.sign = "存储失败";
      await MessagePlugin.error("存储失败");
    } finally {
      loading.value = false;
    }
  }
};
const handleSelect = async () => {
  loading.value = true;
  try {
    if (query.id < 0) {
      query.id = 0;
      await MessagePlugin.error("ID不能小于0");
    } else {
      const r = (await apiPost("/api/save/query", query)).data as SaveEntry;
      const { id, text, note } = r;
      if (id < 0) {
        result.sign = "读取失败";
        await MessagePlugin.error("读取失败");
      } else {
        result.text = query.text = text;
        result.note = query.note = note;
        result.sign = "读取成功";
        saveStore.update(query.id);
        await MessagePlugin.success("读取成功");
      }
    }
  } catch (e) {
    console.error(e);
    result.sign = "读取失败";
    await MessagePlugin.error("读取失败");
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  query.id = saveStore.id;
});
</script>

<template>
  <ContentLayout :title="t('textStorage.tt')" :subtitle="t('textStorage.st')">
    <t-form>
      <t-form-item :label="t('textStorage.form.store')">
        <t-textarea v-model="query.text" :autosize="true" placeholder="存储内容" />
      </t-form-item>
      <t-form-item :label="t('textStorage.form.note')">
        <t-input v-model="query.note" placeholder="存储备注" />
      </t-form-item>
      <t-form-item :label="t('textStorage.form.read')">
        <t-textarea :autosize="true" :readonly="true" :value="result.text" placeholder="读取内容" />
      </t-form-item>
      <t-form-item :label="t('textStorage.form.readNote')">
        <t-input :value="result.note" :readonly="true" placeholder="读取备注" />
      </t-form-item>
      <t-divider />
      <t-form-item label="ID">
        <t-input-number v-model="query.id" :auto-width="true" :min="0" size="small" />
      </t-form-item>
      <t-form-item :label="t('textStorage.form.operation')">
        <t-loading :loading="loading" size="small">
          <t-space>
            <t-button shape="circle" theme="primary" @click="handleStore">
              <UploadIcon />
            </t-button>
            <t-button shape="circle" theme="primary" @click="handleSelect">
              <DownloadIcon />
            </t-button>
          </t-space>
        </t-loading>
      </t-form-item>
      <t-form-item :label="t('textStorage.form.tool')">
        <t-space :size="5">
          <ButtonCopy :target="result.text" />
          <ButtonRead v-model:target="query.text" />
          <ButtonClear v-model:target="query.text" />
        </t-space>
      </t-form-item>
    </t-form>
  </ContentLayout>
</template>

<i18n lang="yaml" locale="en">
textStorage:
  tt: "Text Storage"
  st: "Key-Value"
  form:
    store: "Store Content"
    note: "Store Note"
    read: "Read Content"
    readNote: "Read Note"
    operation: "Data Operation"
    tool: "Tool"
</i18n>

<i18n lang="yaml" locale="zh-CN">
textStorage:
  tt: "字符存储"
  st: "键值对"
  form:
    store: "存储内容"
    note: "存储备注"
    read: "读取内容"
    readNote: "读取备注"
    operation: "数据操作"
    tool: "工具"
</i18n>
