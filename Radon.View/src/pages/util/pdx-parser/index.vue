<script setup lang="tsx">
import { apiPostWithFile } from "@/lib/common/apiMethods";
import { usePdxStore } from "@/pages/util/pdx-parser/scripts/store";
import type { PdxParsedLangItem } from "@/pages/util/pdx-parser/scripts/type";
import { useGlobalStore } from "@/store/global";
import { get, useDark, useToggle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import {
  AddIcon,
  DeleteIcon,
  DownloadIcon,
  HomeIcon,
  RefreshIcon,
  SettingIcon,
  Upload1Icon,
  UploadIcon,
} from "tdesign-icons-vue-next";
import type { RequestMethodResponse, TreeNodeValue, UploadFile } from "tdesign-vue-next";
import { onMounted, ref } from "vue";

const global = useGlobalStore();
const store = usePdxStore();
const [menuUploadVisible, setMenuUploadVisible] = useToggle(false);
const [menuReplaceVisible, setMenuReplaceVisible] = useToggle(false);
const [menuReplaceAddVisible, setMenuReplaceAddVisible] = useToggle(false);
const { initialized, replacer, parseLangResult, addReplacerKey, addReplacerValue } = storeToRefs(store);
const { authPassed } = storeToRefs(global);
const treeVal = ref<TreeNodeValue[]>([]);
const regQuote = /\\"/g;
const dark = useDark();

const requestMethod = async (file: UploadFile): Promise<RequestMethodResponse> => {
  try {
    const response = await apiPostWithFile<PdxParsedLangItem[]>(
      "api/pdx/parse/lang",
      new Blob([file.raw!], { type: file.type }),
    );

    store.updateParsedLang(response.data);
    setMenuUploadVisible(false);

    return { status: "success", response: { success: true } };
  } catch (e) {
    console.error(e);
    return { status: "fail", error: "失败", response: {} };
  }
};
const renderText = (raw: string) => {
  return raw
    .replace(/§([beghlmprstwy])/gi, (_, p1) => `<span class='r-pdx-c${get(dark) ? "-dark" : ""}-${p1.toLowerCase()}'>`)
    .replace(/§!/g, "</span>")
    .replace(
      /\$(\w+)\$/g,
      (_, key) => `<span style="font-weight: bold">${replacer.value[key] ?? "(" + key + ")"}</span>`,
    )
    .replace(/\[(.+?)]/g, (_, key) => `<span>${replacer.value[key] ?? "[" + key + "]"}</span>`);
};

onMounted(() => {
  if (!get(initialized)) {
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
                  .replace(/(\\n)+/, '\\n')
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
      <t-button variant="text" theme="primary" shape="circle" @click="setMenuReplaceVisible()">
        <RefreshIcon />
      </t-button>
      <t-button variant="text" theme="primary" shape="circle" @click="setMenuUploadVisible()">
        <UploadIcon />
      </t-button>
      <RouterLink to="/">
        <t-button variant="text" theme="primary" shape="circle">
          <HomeIcon />
        </t-button>
      </RouterLink>
    </template>
    <!-- Upload Dialog -->
    <t-dialog v-model:visible="menuUploadVisible" header="上传" :footer="false" width="85%">
      <t-space direction="vertical" align="baseline">
        <t-upload
          :request-method="requestMethod"
          theme="file-input"
          placeholder="Paradox Yaml Lang"
          tips="支持YAML，不要反复上传文件，你的浏览器把持不住"
        >
          <t-button variant="outline" theme="default" shape="circle">
            <Upload1Icon />
          </t-button>
        </t-upload>
      </t-space>
    </t-dialog>
    <!-- Replace Dialog -->
    <t-dialog v-model:visible="menuReplaceVisible" header="替换值" width="85%">
      <div v-if="!menuReplaceAddVisible">
        <t-list :stripe="true" :split="true" style="max-height: 280px" size="small">
          <t-list-item v-for="(value, key) in replacer" :key="key">
            <t-list-item-meta :title="key.toString()" :description="value" />
            <template #action>
              <t-button
                shape="circle"
                theme="warning"
                variant="outline"
                size="small"
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
          <t-form-item label="键">
            <t-input v-model="addReplacerKey" />
          </t-form-item>
          <t-form-item label="值">
            <t-input v-model="addReplacerValue" />
          </t-form-item>
          <t-form-item :label="authPassed ? '修改 / 上传 / 读取' : '修改'">
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
        <t-button theme="default" shape="circle" @click="setMenuReplaceAddVisible()">
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

.r-pdx-c-dark-b {
  color: blue;
}
.r-pdx-c-dark-e {
  color: teal;
}
.r-pdx-c-dark-g {
  color: green;
}
.r-pdx-c-dark-h {
  color: orange;
}
.r-pdx-c-dark-l {
  color: brown;
}
.r-pdx-c-dark-m {
  color: purple;
}
.r-pdx-c-dark-p {
  color: orangered;
}
.r-pdx-c-dark-r {
  color: red;
}
.r-pdx-c-dark-s {
  color: darkorange;
}
.r-pdx-c-dark-t {
  color: lightgray;
}
.r-pdx-c-dark-w {
  color: white;
}
.r-pdx-c-dark-y {
  color: yellow;
}
</style>
