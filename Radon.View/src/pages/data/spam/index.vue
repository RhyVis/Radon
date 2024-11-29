<script setup lang="ts">
import { apiPost, apiPutNumber } from "@/lib/common/apiMethods";
import { codeTypes, spamColumns, SpamType, spamTypes, type TextEntry } from "@/pages/data/spam/scripts/define";
import { useSpamStore } from "@/pages/data/spam/scripts/store";
import { useGlobalStore } from "@/store/global";
import { get, set, useClipboard, useToggle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import {
  CloseIcon,
  HomeIcon,
  LoudspeakerIcon,
  PlayCircleStrokeAddIcon,
  RefreshIcon,
  ReplayIcon,
  ToolsIcon,
} from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";

const global = useGlobalStore();
const store = useSpamStore();
const { qType, qDict, qSize, qIds, aType, aText, activeTab } = storeToRefs(store);
const { copy } = useClipboard();
const { t } = useI18n();
const [resultLoading, setResultLoading] = useToggle(false);
const [appendLoading, setAppendLoading] = useToggle(false);
const [appendDialog, setAppendDialog] = useToggle(false);
const [used, setUsed] = useToggle(false);
const precise = ref(false);
const preciseStatus = ref<"default" | "error">("default");

const result = ref<TextEntry[]>([
  { id: 666, text: "快乐生活每一天，请不要用这个工具的结果来攻击他人哦😊" },
  { id: 999, text: "仅供学习交流使用，由您不当使用造成的后果，将由您承担" },
]);
const columns = computed(() => spamColumns(handleEntryCopy));
const tagValid = computed(() => get(qIds).every(item => Number.isInteger(Number(item))));
const mergedResult = computed(() =>
  get(result)
    .map(e => e.text)
    .join("\n"),
);

const handleTabChange = (key: string | number) => {
  switch (key) {
    case "spam":
      set(qType, SpamType.SpamMin);
      break;
    case "mmr":
      set(qType, SpamType.Genshin);
      break;
    case "meme":
      set(qType, SpamType.ACGN);
      break;
    default:
  }
};
const handleTagInput = () => {
  set(preciseStatus, "default");
  if (!get(tagValid)) {
    set(preciseStatus, "error");
    MessagePlugin.warning("请输入正确的ID");
  }
};
const handleEntryCopy = (s: string) => {
  try {
    copy(s).then(() => MessagePlugin.success("复制成功"));
  } catch (e) {
    console.error(e);
    MessagePlugin.error("复制失败");
  }
};
const handleFetch = async () => {
  const p = get(precise);
  if (p && !get(tagValid)) {
    void MessagePlugin.warning("请输入正确的ID");
    return;
  }
  setResultLoading(true);
  try {
    const q = get(precise) ? store.queryPrecise : store.query;
    const u = get(precise) ? "/api/spam/precise" : "/api/spam";
    set(result, (await apiPost<TextEntry[]>(u, q)).data);
    setUsed(true);
  } catch (e) {
    console.error(e);
    await MessagePlugin.error("获取信息失败");
  } finally {
    setResultLoading(false);
  }
};
const handleFetchAgain = async () => {
  if (!get(used)) {
    await MessagePlugin.warning("请先获取一次");
    return;
  }
  const ids = get(result).map(e => e.id);
  setResultLoading(true);
  try {
    set(
      result,
      (
        await apiPost<TextEntry[]>("/api/spam/precise", {
          type: get(qType),
          dict: get(qDict),
          ids: ids,
        })
      ).data,
    );
  } catch (e) {
    console.error(e);
    await MessagePlugin.error("获取信息失败");
  } finally {
    setResultLoading(false);
  }
};
const handleAppend = async (repeat: boolean = false) => {
  if (aText.value.length === 0) {
    await MessagePlugin.warning(t("common.noEmpty"));
    return;
  }
  setAppendLoading(true);
  try {
    const r = (await apiPutNumber("/api/spam/append", store.queryAppend)).data;
    if (r > 0) {
      store.clearAppendQuery();
      void MessagePlugin.success(`追加成功: ${r}`);
    } else {
      void MessagePlugin.error(`追加失败: ${r}`);
    }
  } catch (e) {
    console.error(e);
    await MessagePlugin.error("追加失败");
  } finally {
    setAppendLoading(false);
    if (!repeat) {
      setTimeout(() => setAppendDialog(false), 666);
    }
  }
};

watch(
  () => store.qType,
  () => setUsed(false),
);
</script>

<template>
  <content-layout title="弹药库" subtitle="对线宝典">
    <t-form>
      <t-tabs v-model="activeTab" @change="handleTabChange">
        <!--祖安-->
        <t-tab-panel class="mt-2" label="祖安特区" value="spam">
          <t-form-item label="使用说明">
            <span class="r-sp-panel-desc">高强度版本容易被夹，建议转义</span>
          </t-form-item>
          <t-form-item label="选择强度">
            <t-radio-group v-model="qType">
              <t-radio-button value="sn">小喷怡情😋</t-radio-button>
              <t-radio-button value="sx">火力全开😠</t-radio-button>
            </t-radio-group>
          </t-form-item>
        </t-tab-panel>
        <!--MMR-->
        <t-tab-panel class="mt-2" label="二游笑话" value="mmr">
          <t-form-item label="使用说明">
            <span class="r-sp-panel-desc">
              介于各路孝子挂对面的时候都是截图挂人，
              所以这数据库里面很多东西也都是OCR扫出来的，有错字就当二游痴子没文化吧
            </span>
          </t-form-item>
          <t-form-item label="选择游戏">
            <t-radio-group v-model="qType">
              <t-tooltip content="原神" placement="top">
                <t-radio-button value="gs">原批</t-radio-button>
              </t-tooltip>
              <t-tooltip content="明日方舟" placement="top">
                <t-radio-button value="ak">粥畜</t-radio-button>
              </t-tooltip>
              <t-tooltip content="麻辣香锅" placement="top">
                <t-radio-button value="ml">麻辣</t-radio-button>
              </t-tooltip>
            </t-radio-group>
          </t-form-item>
        </t-tab-panel>
        <!--Meme-->
        <t-tab-panel class="mt-2" label="复制粘贴" value="meme">
          <t-form-item label="使用说明">
            <span class="r-sp-panel-desc">我喜欢复制粘贴</span>
          </t-form-item>
          <t-form-item label="选择主题">
            <t-radio-group v-model="qType" type="button">
              <t-tooltip content="二次元欠图了" placement="top">
                <t-radio-button value="ac">反二圣经</t-radio-button>
              </t-tooltip>
              <t-tooltip content="不知道该怎么分类了" placement="top">
                <t-radio-button value="dn">纯正低能</t-radio-button>
              </t-tooltip>
            </t-radio-group>
          </t-form-item>
        </t-tab-panel>
      </t-tabs>
      <div class="mt-4">
        <t-form-item label="转义方式">
          <SelSimple v-model="qDict" :options="codeTypes" />
        </t-form-item>
        <t-form-item v-if="!precise" label="妙语连珠">
          <t-input-number v-model="qSize" :min="1" :max="10" :auto-width="true" :allow-input-over-limit="false" />
        </t-form-item>
        <t-form-item v-if="precise" label="ID">
          <t-tag-input v-model="qIds" :status="preciseStatus" :clearable="true" @change="handleTagInput" />
        </t-form-item>
        <t-form-item label="精确ID" help="指定选择的条目ID">
          <t-switch v-model="precise" />
        </t-form-item>
        <t-form-item label="开火！">
          <t-space>
            <t-button shape="round" theme="danger" @click="handleFetch" :loading="resultLoading">
              <LoudspeakerIcon v-if="!resultLoading" />
            </t-button>
            <t-tooltip placement="bottom" content="重新获取上一次的内容，可以改变转义模式">
              <t-button
                :disabled="!used"
                shape="circle"
                theme="default"
                @click="handleFetchAgain"
                :loading="resultLoading"
              >
                <RefreshIcon v-if="!resultLoading" />
              </t-button>
            </t-tooltip>
            <btn-copy :value="mergedResult" />
          </t-space>
        </t-form-item>
      </div>
    </t-form>
    <!--结果-->
    <t-divider />
    <div class="mb-2 mt-2" v-if="result.length > 0">
      <t-table
        size="small"
        row-key="id"
        :columns="columns"
        :data="result"
        :stripe="true"
        :hover="true"
        :bordered="true"
        :loading="resultLoading"
      />
    </div>
    <template #actions>
      <t-button v-if="global.authPassed" variant="text" shape="circle" theme="primary" @click="setAppendDialog(true)">
        <ToolsIcon />
      </t-button>
      <RouterLink to="/">
        <t-button variant="text" theme="primary" shape="circle">
          <HomeIcon />
        </t-button>
      </RouterLink>
    </template>
    <t-dialog v-model:visible="appendDialog" header="追加内容" width="75%" @close="store.clearAppendQuery()">
      <t-form label-align="top">
        <t-form-item label="类型">
          <SelSimple v-model="aType" :options="spamTypes" />
        </t-form-item>
        <t-form-item label="内容">
          <t-textarea v-model="aText" />
        </t-form-item>
      </t-form>
      <template #footer>
        <t-space>
          <btn-read v-model="aText" />
          <t-button theme="default" shape="round" @click="setAppendDialog(false)">
            <CloseIcon />
          </t-button>
          <t-button theme="primary" shape="round" @click="handleAppend()" :loading="appendLoading">
            <PlayCircleStrokeAddIcon v-if="!appendLoading" />
          </t-button>
          <t-button theme="primary" shape="round" @click="handleAppend(true)" :loading="appendLoading">
            <ReplayIcon v-if="!appendLoading" />
          </t-button>
        </t-space>
      </template>
    </t-dialog>
  </content-layout>
</template>

<style scoped lang="less">
@import "@/assets/style/mixin.less";

:global(.r-sp-column-tag) {
  text-align: center;
  .r-no-select();
}

.r-sp-panel-desc {
  text-align: left;
}
</style>
