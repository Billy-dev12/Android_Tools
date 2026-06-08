//go:build windows

package detector

import "testing"

func TestParseInstanceId(t *testing.T) {
	tests := []struct {
		input   string
		wantVid string
		wantPid string
		wantSer string
	}{
		{
			input:   `USB\VID_18D1&PID_D00D\84B7N15A07004381`,
			wantVid: "18D1",
			wantPid: "D00D",
			wantSer: "84B7N15A07004381",
		},
		{
			input:   `USB\VID_05C6&PID_9008\5&2d5c328e&0&1`,
			wantVid: "05C6",
			wantPid: "9008",
			wantSer: "5&2d5c328e&0&1",
		},
		{
			input:   `USB\INVALID_FORMAT`,
			wantVid: "",
			wantPid: "",
			wantSer: "",
		},
	}

	for _, tt := range tests {
		gotVid, gotPid, gotSer := parseInstanceId(tt.input)
		if gotVid != tt.wantVid || gotPid != tt.wantPid || gotSer != tt.wantSer {
			t.Errorf("parseInstanceId(%q) = (%q, %q, %q); want (%q, %q, %q)",
				tt.input, gotVid, gotPid, gotSer, tt.wantVid, tt.wantPid, tt.wantSer)
		}
	}
}
