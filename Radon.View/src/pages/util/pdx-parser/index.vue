<script setup lang="tsx">
import axiosInstance from "@/lib/common/apiHttp";
import type { ApiResponse } from "@/lib/type/typeApi";
import { usePdxStore } from "@/pages/util/pdx-parser/scripts/store";
import type { PdxParsedLangItem } from "@/pages/util/pdx-parser/scripts/type";
import { set } from "@vueuse/core";
import { AddIcon, HomeIcon, RefreshIcon, SettingIcon, UploadIcon } from "tdesign-icons-vue-next";
import type { RequestMethodResponse, TreeNodeValue, UploadFile } from "tdesign-vue-next";

const regQuote = /\\"/g;
const treeVal = ref<TreeNodeValue[]>([]);
const uploadMenuVisible = ref(false);
const replaceMenuVisible = ref(false);
const replaceMenuAddVisible = ref(false);
const store = usePdxStore();
const { initialized, replacer, parseLangResult, addReplacerKey, addReplacerValue } = storeToRefs(store);

const requestMethod = async (file: UploadFile): Promise<RequestMethodResponse> => {
  try {
    const formData = new FormData();
    formData.append("file", new Blob([file.raw!], { type: file.type }));
    const res = await axiosInstance.post("api/pdx/parse/lang", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    store.updateParsedLang((res.data as ApiResponse<PdxParsedLangItem[]>).data);
    set(uploadMenuVisible, false);
    return { status: "success", response: { success: true } };
  } catch (e) {
    console.error(e);
    return { status: "fail", error: "失败", response: {} };
  }
};
const renderText = (raw: string) => {
  return raw
    .replace(/§([beghlmprstwy])/gi, (_, p1) => `<span class='r-pdx-c-${p1.toLowerCase()}'>`)
    .replace(/§!/g, "</span>")
    .replace(/\$(\w+)\$/g, (_, key) => `<span style="font-weight: bold">${replacer.value[key] ?? key}</span>`)
    .replace(/\[(.+?)]/g, (_, key) => `<span>${replacer.value[key] ?? key}</span>`);
};

onMounted(() => {
  if (!initialized.value) {
    store.init();
  }
});
</script>

<template>
  <content-layout title="PDX Parser">
    <div class="r-pdx-container">
      <div>
        <t-card class="r-pdx-cd">
          <div class="r-pdx-cd-content">
            <t-space direction="vertical">
              <div
                v-for="(item, index) in (treeVal[0] ?? '')
                  .toString()
                  .replace(regQuote, '')
                  .replace('\\n\\n', '\\n')
                  .split('\\n')"
                v-html="renderText(item)"
                :key="index"
              />
            </t-space>
          </div>
        </t-card>
        <t-divider />
      </div>
      <div class="r-pdx-inner">
        <t-space>
          <t-tree
            v-model:actived="treeVal"
            :data="parseLangResult"
            :activable="true"
            :hover="true"
            :line="true"
            :transition="true"
          />
        </t-space>
      </div>
    </div>
    <template #actions>
      <t-button variant="text" theme="primary" shape="circle" size="small" @click="replaceMenuVisible = true">
        <RefreshIcon />
      </t-button>
      <t-button variant="text" theme="primary" shape="circle" size="small" @click="uploadMenuVisible = true">
        <UploadIcon />
      </t-button>
      <RouterLink to="/">
        <t-button variant="text" theme="primary" shape="circle" size="small">
          <HomeIcon />
        </t-button>
      </RouterLink>
    </template>
    <t-dialog v-model:visible="uploadMenuVisible" header="上传" :footer="false" width="85%">
      <t-space direction="vertical" align="baseline">
        <t-upload :request-method="requestMethod" theme="file-input" placeholder="解析Paradox Yaml Lang" />
        <t-text>支持YAML，不要反复上传文件，你的浏览器把持不住</t-text>
      </t-space>
    </t-dialog>
    <t-dialog v-model:visible="replaceMenuVisible" header="替换值" width="85%">
      <t-list :stripe="true" :split="true" style="max-height: 250px" size="small">
        <template #header> 用于替换默认的键值对 </template>
        <t-list-item v-for="(_, key) in replacer" :key="key">
          <t-list-item-meta :title="key.toString()" :description="replacer[key]" />
        </t-list-item>
      </t-list>
      <Transition name="move">
        <div v-if="replaceMenuAddVisible">
          <t-divider />
          <t-form label-align="top">
            <t-form-item label="键">
              <t-input v-model="addReplacerKey" />
            </t-form-item>
            <t-form-item label="值">
              <t-input v-model="addReplacerValue" />
            </t-form-item>
            <t-form-item label="添加/修改">
              <t-button shape="circle" @click="store.updateReplacer()">
                <AddIcon />
              </t-button>
            </t-form-item>
          </t-form>
        </div>
      </Transition>
      <template #footer>
        <t-button theme="default" shape="circle" @click="replaceMenuAddVisible = !replaceMenuAddVisible">
          <SettingIcon />
        </t-button>
      </template>
    </t-dialog>
  </content-layout>
</template>

<style scoped lang="less">
.r-pdx-container {
  .r-pdx-cd {
    max-width: 100%;
    height: 34vh;
    overflow: auto;

    .r-pdx-cd-content {
      height: 100%;
      overflow-y: auto;
    }
  }
  .r-pdx-inner {
    height: 33vh;
    overflow: auto;
    scrollbar-width: thin;
  }
}

.move-enter-active {
  transition: all 0.3s ease-out;
}
.move-leave-active {
  transition: all 0.3s cubic-bezier(1, 0.5, 0.8, 1);
}
.move-enter-from,
.move-leave-to {
  transform: translateX(-20px);
  opacity: 0;
}
</style>

<style lang="less">
.r-pdx-c-b {
  color: blue;
}
.r-pdx-c-e {
  color: teal;
}
.r-pdx-c-g {
  color: green;
}
.r-pdx-c-h {
  color: orange;
}
.r-pdx-c-l {
  color: brown;
}
.r-pdx-c-m {
  color: purple;
}
.r-pdx-c-p {
  color: orangered;
}
.r-pdx-c-r {
  color: red;
}
.r-pdx-c-s {
  color: darkorange;
}
.r-pdx-c-t {
  color: grey;
}
.r-pdx-c-w {
  font-style: italic;
}
.r-pdx-c-y {
  color: yellow;
}
</style>
